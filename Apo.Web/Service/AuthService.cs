using Apo.Web.Models;
using Apo.Web.Service.IService;

namespace Apo.Web.Service
{
    public class AuthService : IAuthService
    {
        private IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> AssingneRole(RegistrationRequestDTO assingneRoleDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Apo.Web.Utility.SD.ApiType.POST,
                Data = assingneRoleDto,
                Url = Apo.Web.Utility.SD.AuthAPIBase + $"/api/auth/assing-role"
            });
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDTO loginDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Apo.Web.Utility.SD.ApiType.POST,
                Data = loginDto,
                Url = Apo.Web.Utility.SD.AuthAPIBase + $"/api/auth/login"
            });
        }

        public async Task<ResponseDto?> Register(RegistrationRequestDTO registerDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Apo.Web.Utility.SD.ApiType.POST,
                Data = registerDto,
                Url = Apo.Web.Utility.SD.AuthAPIBase + $"/api/auth/register"
            });
        }
    }
}
