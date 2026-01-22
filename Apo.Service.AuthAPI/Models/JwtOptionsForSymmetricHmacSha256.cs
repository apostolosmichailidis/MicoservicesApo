namespace Apo.Services.AuthAPI.Models
{
    public class JwtOptionsForSymmetricHmacSha256
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
    }
}
