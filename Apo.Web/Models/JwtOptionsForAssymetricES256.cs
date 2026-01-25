namespace Apo.Web.Models
{
    public class JwtOptionsForAssymetricES256
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string PublicKeyPath { get; set; }
    }
}
