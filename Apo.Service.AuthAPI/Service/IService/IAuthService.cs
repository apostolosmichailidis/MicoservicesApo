using Apo.Service.AuthAPI.Models.Dto;

namespace Apo.Service.AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDTO registrationRequestDTO);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<bool> AssingRole(string email, string roleName);
    }
}
