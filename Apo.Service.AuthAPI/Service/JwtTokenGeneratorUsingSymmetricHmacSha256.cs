using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Apo.Service.AuthAPI.Models;
using Apo.Service.AuthAPI.Service.IService;
using Apo.Services.AuthAPI.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Apo.Service.AuthAPI.Service
{
    public class JwtTokenGeneratorUsingSymmetricHmacSha256 : IJWTTokenGenerator
    {
        private readonly JwtOptionsForSymmetricHmacSha256 _jwtOptions;

        public JwtTokenGeneratorUsingSymmetricHmacSha256(IOptions<JwtOptionsForSymmetricHmacSha256> jwtOptions) 
        {
            _jwtOptions = jwtOptions.Value;
        }
        public string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

            var claimList = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Name, applicationUser.Name),
                new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email),
                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id)
            };

            claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
