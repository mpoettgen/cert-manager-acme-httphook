using System;
using Microsoft.Extensions.Options;
using Renci.SshNet;

namespace CertManager.Acme.HttpHook
{
    public class SftpClientFactory : ISftpClientFactory
    {
        private readonly SftpClientOptions _sftpClientOptions;

        public SftpClientFactory(IOptions<SftpClientOptions> sftpClientOptions)
        {
            _sftpClientOptions = sftpClientOptions.Value ?? throw new ArgumentNullException(nameof(sftpClientOptions));
        }

        public SftpClient CreateClient()
        {
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
