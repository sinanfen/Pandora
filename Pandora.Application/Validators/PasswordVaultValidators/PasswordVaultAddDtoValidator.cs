using FluentValidation;
using Pandora.Shared.DTOs.PasswordVaultDTOs;

namespace Pandora.Application.Validators.PasswordVaultValidators;

public class PasswordVaultAddDtoValidator : AbstractValidator<PasswordVaultAddDto>
{
    public PasswordVaultAddDtoValidator()
    {
        // UserId field validation
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Kullanıcı kimliği boş olamaz.");

        // SiteName field validation
        RuleFor(x => x.SiteName)
            .NotEmpty().WithMessage("Site adı boş olamaz.")
            .MinimumLength(3).WithMessage("Site adı en az 3 karakter olmalıdır.");

        // UsernameOrEmail field validation
        RuleFor(x => x.UsernameOrEmail)
            .NotEmpty().WithMessage("Kullanıcı adı veya email boş olamaz.")
            .MinimumLength(5).WithMessage("Kullanıcı adı veya email en az 5 karakter olmalıdır.");

        // Password field validation
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre boş olamaz.")
            .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.")
            .Matches("[A-Z]").WithMessage("Şifre en az bir büyük harf içermelidir.")
            .Matches("[a-z]").WithMessage("Şifre en az bir küçük harf içermelidir.")
            .Matches("[0-9]").WithMessage("Şifre en az bir rakam içermelidir.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Şifre en az bir özel karakter içermelidir."); // Özel karakter zorunluluğu

        RuleFor(x => x.PasswordRepeat)
            .Equal(x => x.Password).WithMessage("Şifre tekrarı, şifreyle aynı olmalıdır.");
        //// PasswordExpirationDate validation (optional)
        //RuleFor(x => x.PasswordExpirationDate)
        //    .GreaterThan(DateTime.UtcNow).WithMessage("Şifre son kullanma tarihi bugünden ileri bir tarih olmalıdır.")
        //    .When(x => x.PasswordExpirationDate.HasValue);

        // CategoryId validation (optional)
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Geçerli bir kategori seçilmelidir.");
    }
}

