using FluentValidation;
using Pandora.Application.DTOs.UserDTOs;

namespace Pandora.Application.Validators.UserValidators;

public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
{
    public UserUpdateDtoValidator()
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

        //// Corporate-specific Validation (if applicable)
        //RuleFor(x => x.CompanyName)
        //    .NotEmpty().When(x => x.TaxNumber != null).WithMessage("Şirket adı boş olamaz.")
        //    .Length(3, 100).WithMessage("Şirket adı 3 ile 100 karakter arasında olmalıdır.");

        //RuleFor(x => x.TaxNumber)
        //    .NotEmpty().When(x => x.CompanyName != null).WithMessage("Vergi numarası boş olamaz.")
        //    .Length(10, 20).WithMessage("Vergi numarası 10 ile 20 karakter arasında olmalıdır.");

        // Individual-specific Validation
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ad boş olamaz.")
            .Length(2, 50).WithMessage("Ad 2 ile 50 karakter arasında olmalıdır.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Soyad boş olamaz.")
            .Length(2, 50).WithMessage("Soyad 2 ile 50 karakter arasında olmalıdır.");
    }
}
