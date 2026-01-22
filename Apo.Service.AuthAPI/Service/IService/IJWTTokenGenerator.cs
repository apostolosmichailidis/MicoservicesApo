using Apo.Service.AuthAPI.Models;

namespace Apo.Service.AuthAPI.Service.IService
{
    public interface IJWTTokenGenerator
    {
        string GenerateToken(ApplicationUser application, IEnumerable<string> roles);
    }
}
