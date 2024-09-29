using FluentValidation;
using Pandora.Application.DTOs.PasswordVaultDTOs;

namespace Pandora.Application.Validators.PasswordVaultValidators
{
    public class PasswordVaultUpdateDtoValidator : AbstractValidator<PasswordVaultUpdateDto>
    {
        public PasswordVaultUpdateDtoValidator()
        {
            // SiteName field validation
            RuleFor(x => x.SiteName)
                .NotEmpty().WithMessage("Site adı boş olamaz.")
                .MinimumLength(3).WithMessage("Site adı en az 3 karakter olmalıdır.");

            // UsernameOrEmail field validation
            RuleFor(x => x.UsernameOrEmail)
                .NotEmpty().WithMessage("Kullanıcı adı veya email boş olamaz.")
                .MinimumLength(5).WithMessage("Kullanıcı adı veya email en az 5 karakter olmalıdır.");

            //// PasswordExpirationDate validation
            //RuleFor(x => x.PasswordExpirationDate)
            //    .GreaterThan(DateTime.UtcNow).WithMessage("Şifre son kullanma tarihi bugünden ileri bir tarih olmalıdır.")
            //    .When(x => x.PasswordExpirationDate.HasValue);

            //// LastPasswordChangeDate validation
            //RuleFor(x => x.LastPasswordChangeDate)
            //    .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Son şifre değiştirme tarihi geçersiz.")
            //    .When(x => x.LastPasswordChangeDate.HasValue);

            // CategoryId validation
            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Geçerli bir kategori seçilmelidir.")
                .When(x => x.CategoryId.HasValue);
        }
    }
}
