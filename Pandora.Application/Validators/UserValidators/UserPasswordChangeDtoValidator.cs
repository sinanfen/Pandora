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

        // NewPassword validation (password complexity)
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Yeni şifre boş olamaz.")
            .MinimumLength(8).WithMessage("Yeni şifre en az 8 karakter olmalıdır.")
            .Matches("[A-Z]").WithMessage("Yeni şifre en az bir büyük harf içermelidir.")
            .Matches("[a-z]").WithMessage("Yeni şifre en az bir küçük harf içermelidir.")
            .Matches("[0-9]").WithMessage("Yeni şifre en az bir rakam içermelidir.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Yeni şifre en az bir özel karakter içermelidir.");

        // PasswordRepeat validation (password confirmation)
        RuleFor(x => x.ConfirmNewPassword)
            .Equal(x => x.NewPassword).WithMessage("Şifre tekrarı, şifre ile aynı olmalıdır.");
    }
}
