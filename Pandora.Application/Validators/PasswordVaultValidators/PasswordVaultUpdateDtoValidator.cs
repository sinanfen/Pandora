using FluentValidation;
using Pandora.Shared.DTOs.PasswordVaultDTOs;

namespace Pandora.Application.Validators.PasswordVaultValidators
{
    public class PasswordVaultUpdateDtoValidator : AbstractValidator<PasswordVaultUpdateDto>
    {
        public PasswordVaultUpdateDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("ID boş olamaz.")
            .NotNull().WithMessage("ID geçersiz.");

            // SiteName field validation
            RuleFor(x => x.SiteName)
                .NotEmpty().WithMessage("Site adı boş olamaz.")
                .MinimumLength(3).WithMessage("Site adı en az 3 karakter olmalıdır.");

            // UsernameOrEmail field validation
            RuleFor(x => x.UsernameOrEmail)
                .NotEmpty().WithMessage("Kullanıcı adı veya email boş olamaz.")
                .MinimumLength(5).WithMessage("Kullanıcı adı veya email en az 5 karakter olmalıdır.");

            // CurrentPassword field validation
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mevcut şifre boş olamaz.")
                .MinimumLength(8).WithMessage("Mevcut şifre en az 8 karakter olmalıdır.");

            // NewPassword field validation
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Yeni şifre boş olamaz.")
                .MinimumLength(8).WithMessage("Yeni şifre en az 8 karakter olmalıdır.")
                .Matches("[A-Z]").WithMessage("Yeni şifre en az bir büyük harf içermelidir.")
                .Matches("[a-z]").WithMessage("Yeni şifre en az bir küçük harf içermelidir.")
                .Matches("[0-9]").WithMessage("Yeni şifre en az bir rakam içermelidir.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Yeni şifre en az bir özel karakter içermelidir.");

            // NewPasswordRepeat must match NewPassword
            RuleFor(x => x.NewPasswordRepeat)
                .Equal(x => x.NewPassword).WithMessage("Yeni şifre tekrarı, yeni şifreyle aynı olmalıdır.");

            // CategoryId validation
            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Geçerli bir kategori seçilmelidir.")
                .When(x => x.CategoryId.HasValue);
        }
    }
}
