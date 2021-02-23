using System.ComponentModel.DataAnnotations;

namespace CertManager.Acme.HttpHook
{
    public class SftpClientOptions
    {
        [Required]
        public string Host { get; set; }

        [Required]
        public int Port { get; set; } = 22;

        public string Directory { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
