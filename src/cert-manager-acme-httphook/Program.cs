using System;
using System.IO;
using System.Threading.Tasks;
using k8s;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CertManager.Acme.HttpHook
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args)
                .RunConsoleAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostConext, config) =>
                {
                    string appPath = Directory.GetCurrentDirectory();
                    // Put regular configuration into the `config` folder
                    config.SetBasePath(Path.Combine(appPath, "config"));
                    // Provide for a means to configure secrets in the `secrets` folder.
                    config.AddJsonFile(
                        Path.Combine(appPath, "secrets", "appsettings.secrets.json"),
                        optional: true,
                        reloadOnChange: false
                        );
                    config.AddUserSecrets<Program>(true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var config = KubernetesClientConfiguration.BuildDefaultConfig();
                    services.AddSingleton(config);

                    // Setup the http client
                    services.AddHttpClient("K8s")
                        .AddTypedClient<IKubernetes>((httpClient, serviceProvider) =>
                        {
                            httpClient.Timeout = TimeSpan.FromMinutes(5);
                            return new Kubernetes(
                                serviceProvider.GetRequiredService<KubernetesClientConfiguration>(),
                                httpClient
                                );
                        })
                        .ConfigurePrimaryHttpMessageHandler(config.CreateDefaultHttpClientHandler)
                        .AddHttpMessageHandler(KubernetesClientConfiguration.CreateWatchHandler);
                    services.AddHostedService<ChallengeOperator>();
                    services.AddOptions<SftpClientOptions>()
                        .Bind(hostContext.Configuration.GetSection("SftpClient"))
                        .ValidateDataAnnotations();
                    services.AddSingleton<ISftpClientFactory, SftpClientFactory>();
                });
    }
}
