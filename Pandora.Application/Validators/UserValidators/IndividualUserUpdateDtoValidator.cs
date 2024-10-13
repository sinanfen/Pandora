using FluentValidation;
using Pandora.Shared.DTOs.UserDTOs;

public class IndividualUserUpdateDtoValidator : AbstractValidator<IndividualUserUpdateDto>
{
    public IndividualUserUpdateDtoValidator()
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

        // Individual-specific Validation
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ad boş olamaz.")
            .Length(2, 50).WithMessage("Ad 2 ile 50 karakter arasında olmalıdır.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Soyad boş olamaz.")
            .Length(2, 50).WithMessage("Soyad 2 ile 50 karakter arasında olmalıdır.");
    }
}
