using FluentValidation;
using Pandora.Shared.DTOs.PasswordVaultDTOs;

namespace Pandora.Application.Validators.PasswordVaultValidators
{
    public class PasswordVaultUpdateDtoValidator : AbstractValidator<PasswordVaultUpdateDto>
    {
        public PasswordVaultUpdateDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("ID cannot be empty.")
            .NotNull().WithMessage("ID is invalid.");

            // SiteName field validation
            RuleFor(x => x.SiteName)
                .NotEmpty().WithMessage("Site name cannot be empty.")
                .MinimumLength(3).WithMessage("Site name must be at least 3 characters long.");

            // UsernameOrEmail field validation
            RuleFor(x => x.UsernameOrEmail)
                .NotEmpty().WithMessage("Username or email cannot be empty.");

            // CurrentPassword field validation - Simplified for password manager usage
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Current password cannot be empty.");

            // NewPassword field validation - Simplified for password manager usage
            // Users should be able to update to any password regardless of strength
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password cannot be empty.");

            // NewPasswordRepeat must match NewPassword
            RuleFor(x => x.NewPasswordRepeat)
                .Equal(x => x.NewPassword).WithMessage("New password confirmation must match the new password.");

            // CategoryId validation
            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Please select a valid category.")
                .When(x => x.CategoryId.HasValue);
        }
    }
}
