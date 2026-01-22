using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Apo.Service.AuthAPI.Models;
using Apo.Service.AuthAPI.Service.IService;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Apo.Service.AuthAPI.Service
{
    public class JwtTokenGeneratorUsingES256 : IJWTTokenGenerator
    {
        private readonly JwtOptionsForAssymetricES256 _jwtOptions;
        private readonly ECDsa _ecdsa;

        public JwtTokenGeneratorUsingES256(IOptions<JwtOptionsForAssymetricES256> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;

            // Load private key from PEM
            var privateKeyPem = File.ReadAllText(_jwtOptions.PrivateKeyPath);
            _ecdsa = ECDsa.Create();
            _ecdsa.ImportFromPem(privateKeyPem);
        }

        public string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claimList = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Name, applicationUser.Name),
                new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email),
                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id)
            };

            claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var signingKey = new ECDsaSecurityKey(_ecdsa);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.EcdsaSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
