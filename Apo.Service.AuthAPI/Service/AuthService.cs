using Apo.Service.AuthAPI.Models;
using Apo.Service.AuthAPI.Models.Dto;
using Apo.Service.AuthAPI.Service.IService;
using Apo.Services.AuthAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Apo.Service.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJWTTokenGenerator _jWTTokenGenerator;

        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJWTTokenGenerator jWTTokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jWTTokenGenerator = jWTTokenGenerator;
        }

        public async Task<bool> AssingRole(string email, string roleName)
        {
            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName.ToLower() == email.ToLower());

            if (user != null) 
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist) 
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }

                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var emptyResponse = new LoginResponseDTO() { User = null, Token = "" };
            if (loginRequestDTO == null)
            {
                return emptyResponse;
            }

            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());
            if (user == null) 
            {
                return emptyResponse;
            }

            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
            if (!isPasswordValid)
            {
                return emptyResponse;
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jWTTokenGenerator.GenerateToken(user, roles);

            UserDTO userDTO = new UserDTO()
            {
                ID = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                User = userDTO,
                Token = token
            };

            return loginResponseDTO;

            throw new NotImplementedException();
        }

        public async Task<string> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            ApplicationUser applicationUser = new ApplicationUser()
            {
                UserName = registrationRequestDTO.Email,
                Email = registrationRequestDTO.Email,
                NormalizedEmail = registrationRequestDTO.Email.ToUpper(),
                Name = registrationRequestDTO.Name,
                PhoneNumber = registrationRequestDTO.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, registrationRequestDTO.Password);
                if (result.Succeeded)
                {
                    var userToReturn = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == registrationRequestDTO.Email);
                    
                    if (userToReturn != null) 
                    {
                        UserDTO userDTO = new UserDTO()
                        {
                            ID = userToReturn.Id,
                            Name = userToReturn.Name,
                            Email = userToReturn.Email,
                            PhoneNumber = userToReturn.PhoneNumber
                        };
                        return string.Empty;
                    }
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return String.Empty;
        }
    }
}
