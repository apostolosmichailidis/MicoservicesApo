using FluentValidation;
using Apo.Service.AuthAPI.Models.Dto;

namespace Apo.Service.AuthAPI.Validators
{
    public class AssingRoleRequestValidator : AbstractValidator<AssingRoleRequestDTO>
    {
        public AssingRoleRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required")
                .Must(role => !string.IsNullOrWhiteSpace(role))
                .WithMessage("Role cannot be empty or whitespace");
        }
    }
}
