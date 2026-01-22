namespace Apo.Service.AuthAPI.Models
{
    public class JwtOptionsForAssymetricES256
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }

        // Paths to PEM files
        public string PrivateKeyPath { get; set; }
        public string PublicKeyPath { get; set; }
    }

}
