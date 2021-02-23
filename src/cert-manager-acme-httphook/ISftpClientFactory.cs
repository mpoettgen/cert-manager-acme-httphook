using Renci.SshNet;

namespace CertManager.Acme.HttpHook
{
    public interface ISftpClientFactory
    {
        SftpClient CreateClient();
    }
}