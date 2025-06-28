using FluentValidation;
using Pandora.Shared.DTOs.UserDTOs;

namespace Pandora.Application.Validators.UserValidators;

public class UserPasswordChangeDtoValidator : AbstractValidator<UserPasswordChangeDto>
{
    public UserPasswordChangeDtoValidator()
    {
        // CurrentPassword validation
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Current password cannot be empty.");
   
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Password cannot be empty.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"\d").WithMessage("Password must contain at least one digit.")
            .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");

        // PasswordRepeat validation (password confirmation)
        RuleFor(x => x.ConfirmNewPassword)
            .Equal(x => x.NewPassword).WithMessage("Password confirmation must match the password.");
    }
}
