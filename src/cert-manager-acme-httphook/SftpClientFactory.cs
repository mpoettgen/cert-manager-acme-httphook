using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Renci.SshNet;

namespace CertManager.Acme.HttpHook
{
    public class SftpClientFactory : ISftpClientFactory
    {
        private readonly SftpClientOptions _sftpClientOptions;
        private readonly ILogger _logger;

        public SftpClientFactory(IOptions<SftpClientOptions> sftpClientOptions, ILogger<ChallengeOperator> logger)
        {
            _sftpClientOptions = sftpClientOptions.Value ?? throw new ArgumentNullException(nameof(sftpClientOptions));
            _logger = logger;
        }

        public SftpClient CreateClient()
        {
            _logger.LogInformation(
                "Connecting to sftp://{User}@{Host}:{Port}{Directory}",
                _sftpClientOptions.Username,
                _sftpClientOptions.Host,
                _sftpClientOptions.Port,
                _sftpClientOptions.Directory
                );
            ConnectionInfo connectionInfo = new ConnectionInfo(
                _sftpClientOptions.Host,
                _sftpClientOptions.Port,
                _sftpClientOptions.Username,
                new PasswordAuthenticationMethod(_sftpClientOptions.Username, _sftpClientOptions.Password)
                );
            SftpClient client = new SftpClient(connectionInfo);
            client.Connect();
            if (!string.IsNullOrEmpty(_sftpClientOptions.Directory))
                client.ChangeDirectory(_sftpClientOptions.Directory);
            return client;
        }
    }
}
