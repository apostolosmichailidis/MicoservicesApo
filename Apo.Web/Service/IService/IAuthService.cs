using Apo.Web.Models;

namespace Apo.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto?> LoginAsync(LoginRequestDTO loginDto);

        Task<ResponseDto?> Register(RegistrationRequestDTO loginDto);

        Task<ResponseDto?> AssingneRole(RegistrationRequestDTO loginDto);
    }
}
