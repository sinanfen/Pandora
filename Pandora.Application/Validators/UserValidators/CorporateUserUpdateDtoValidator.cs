using FluentValidation;
using Pandora.Shared.DTOs.UserDTOs;

public class CorporateUserUpdateDtoValidator : AbstractValidator<CorporateUserUpdateDto>
{
    public CorporateUserUpdateDtoValidator()
    {
        // Username Validation
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Kullanıcı adı boş olamaz.")
            .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır.");

        // Email Validation
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email boş olamaz.")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");

        // Phone Number Validation
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefon numarası boş olamaz.");

        // Corporate-specific Validation
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Şirket adı boş olamaz.")
            .Length(3, 100).WithMessage("Şirket adı 3 ile 100 karakter arasında olmalıdır.");

        RuleFor(x => x.TaxNumber)
            .NotEmpty().WithMessage("Vergi numarası boş olamaz.")
            .Length(10, 20).WithMessage("Vergi numarası 10 ile 20 karakter arasında olmalıdır.");
    }
}
