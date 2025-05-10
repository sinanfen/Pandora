using FluentValidation;
using Pandora.Shared.DTOs.UserDTOs;

namespace Pandora.Application.Validators.UserValidators;

public class UserPasswordChangeDtoValidator : AbstractValidator<UserPasswordChangeDto>
{
    public UserPasswordChangeDtoValidator()
    {
        // CurrentPassword validation
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Mevcut şifre boş olamaz.");
   
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Parola boş olamaz.")
            .MinimumLength(8).WithMessage("Parola en az 8 karakter olmalıdır.")
            .Matches(@"[A-Z]").WithMessage("Parola en az bir büyük harf içermelidir.")
            .Matches(@"[a-z]").WithMessage("Parola en az bir küçük harf içermelidir.")
            .Matches(@"\d").WithMessage("Parola en az bir rakam içermelidir.")
            .Matches(@"[\W_]").WithMessage("Parola en az bir özel karakter içermelidir.");

        // PasswordRepeat validation (password confirmation)
        RuleFor(x => x.ConfirmNewPassword)
            .Equal(x => x.NewPassword).WithMessage("Şifre tekrarı, şifre ile aynı olmalıdır.");
    }
}
