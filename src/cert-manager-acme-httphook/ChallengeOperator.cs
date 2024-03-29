﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using k8s;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace CertManager.Acme.HttpHook
{
    public class ChallengeOperator : BackgroundService
    {
        private readonly IKubernetes _client;
        private readonly ISftpClientFactory _sftpClientFactory;
        private readonly ILogger _logger;
        private string _latestResourceVersion;

        public ChallengeOperator(IKubernetes kubernetesClient, ISftpClientFactory sftpClientFactory, ILogger<ChallengeOperator> logger)
        {
            _client = kubernetesClient;
            _sftpClientFactory = sftpClientFactory;
            _logger = logger;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _latestResourceVersion = null;

                await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
                _logger.LogInformation("Challenge operator starting at {time}", DateTimeOffset.Now);

                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogTrace("Challenge operator starting at {time} with resourceVersion {resourceVersion}", DateTimeOffset.Now, _latestResourceVersion);

                    using (Task<HttpOperationResponse<object>> response = _client.ListClusterCustomObjectWithHttpMessagesAsync(
                        group: "acme.cert-manager.io",
                        version: "v1",
                        plural: "challenges",
                        timeoutSeconds: (int)TimeSpan.FromMinutes(5).TotalSeconds,
                        watch: true,
                        cancellationToken: stoppingToken,
                        resourceVersion: _latestResourceVersion
                        ))
                    {
                        _logger.LogTrace("Start watching");
                        await foreach ((WatchEventType type, Challenge item) in response.WatchAsync<Challenge, object>(OnError).WithCancellation(stoppingToken))
                        {
                            _latestResourceVersion = item.Metadata.ResourceVersion;
                            _logger.LogTrace("{type}: {name} (rev {revision}) - {state} - {reason}", type, item.Metadata?.Name, item.Metadata?.ResourceVersion, item.Status?.State, item.Status?.Reason);
                            if (item.Spec?.Solver?.HttpSolver != null)
                            {
                                switch (type)
                                {
                                    case WatchEventType.Added:
                                        HttpChallengeAdded(item);
                                        break;
                                    case WatchEventType.Deleted:
                                        HttpChallengeDeleted(item);
                                        break;
                                }
                            }
                        }
                        _logger.LogTrace("Done with the watcher");

                        if (!response.IsCompleted)
                        {
                            try
                            {
                                await response;    // wait for the response task to finish, so that we can clean it up
                            }
                            catch (Exception ex) when (IsTaskCanceled(ex))
                            {
                                _logger.LogTrace("Response terminated or stop has been requested.");
                            }
                        }
                        _logger.LogTrace("Done with the response");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An unexpected exception has been thrown.");
            }
            finally
            {
                _logger.LogInformation("Background service is terminating.");
            }
        }

        private static bool IsTaskCanceled(Exception ex)
        {
            if (ex is TaskCanceledException)
                return true;
            if (ex is AggregateException aggregate)
                return !aggregate.Flatten().InnerExceptions.Any(iex => !(iex is TaskCanceledException));
            return false;
        }

        private void HttpChallengeAdded(Challenge item)
        {
            _logger.LogInformation("Added {item}", item.ToString());
            CreateChallengeFile(item.Spec?.Token, item.Spec?.Key);
        }

        private void HttpChallengeDeleted(Challenge item)
        {
            _logger.LogInformation("Deleted {item}", item.ToString());
            RemoveChallengeFile(item.Spec?.Token);
        }

        protected virtual void OnError(Exception exception)
        {
            if (IsTaskCanceled(exception))
                _logger.LogTrace("Response terminated.");
            else
                _logger.LogWarning(exception, "Error watching.");
        }

        public void CreateChallengeFile(string filename, string contents)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentException("Must not be null or empty!", nameof(filename));
            if (string.IsNullOrEmpty(contents))
                throw new ArgumentException("Must not be null or empty!", nameof(contents));
            using (SftpClient client = _sftpClientFactory.CreateClient())
            {
                if (!client.Exists(".well-known/acme-challenge"))
                {
                    if (!client.Exists(".well-known"))
                        client.CreateDirectory(".well-known");
                    client.CreateDirectory(".well-known/acme-challenge");
                }
                client.ChangeDirectory(".well-known/acme-challenge");
                client.WriteAllText(string.Concat(client.WorkingDirectory, "/", filename), contents);
            }
            _logger.LogInformation("Challenge presented.");
        }

        public void RemoveChallengeFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentException("Must not be null or empty!", nameof(filename));
            using (SftpClient client = _sftpClientFactory.CreateClient())
            {
                if (client.Exists(".well-known/acme-challenge"))
                {
                    client.ChangeDirectory(".well-known/acme-challenge");
                    client.DeleteFile(string.Concat(client.WorkingDirectory, "/", filename));
                }
            }
            _logger.LogInformation("Challenge removed.");
        }
    }
}
