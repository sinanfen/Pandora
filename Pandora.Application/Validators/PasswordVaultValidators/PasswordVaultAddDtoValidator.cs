using FluentValidation;
using Pandora.Shared.DTOs.PasswordVaultDTOs;

namespace Pandora.Application.Validators.PasswordVaultValidators;

public class PasswordVaultAddDtoValidator : AbstractValidator<PasswordVaultAddDto>
{
    public PasswordVaultAddDtoValidator()
    {
        // SiteName field validation
        RuleFor(x => x.SiteName)
            .NotEmpty().WithMessage("Site name cannot be empty.")
            .MinimumLength(3).WithMessage("Site name must be at least 3 characters long.");

        // UsernameOrEmail field validation
        RuleFor(x => x.UsernameOrEmail)
            .NotEmpty().WithMessage("Username or email cannot be empty.");

        // Password field validation - Simplified for password manager usage
        // Users should be able to store any existing password regardless of strength
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty.");

        // Password repeat validation
        RuleFor(x => x.PasswordRepeat)
            .Equal(x => x.Password).WithMessage("Password confirmation must match the password.");

        // CategoryId validation (optional)
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Please select a valid category.");
    }
}

