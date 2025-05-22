using FluentValidation;
using Pandora.Shared.DTOs.PersonalVaultDTOs;

namespace Pandora.Application.Validators.PersonalVaultValidators;

public class PersonalVaultUpdateDtoValidator : AbstractValidator<PersonalVaultUpdateDto>
{
    public PersonalVaultUpdateDtoValidator()
    {

        RuleFor(x=>x.Id).NotEmpty().WithMessage("ID boş olamaz.")
            .NotNull().WithMessage("ID geçersiz.");

        // Title field validation
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Başlık boş olamaz.")
            .MinimumLength(5).WithMessage("Başlık en az 5 karakter olmalıdır.");

        // Content field validation
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("İçerik boş olamaz.")
            .MinimumLength(10).WithMessage("İçerik en az 10 karakter olmalıdır.");

        // URL field validation
        RuleFor(x => x.Url)
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute)).WithMessage("Geçerli bir URL giriniz.")
            .When(x => !string.IsNullOrEmpty(x.Url));

        // Tags field validation
        RuleFor(x => x.Tags)
            .Must(tags => tags != null && tags.Any()).WithMessage("En az bir etiket eklemelisiniz.")
            .When(x => x.Tags != null && x.Tags.Any());

        // CategoryId validation
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Kategori ID geçersiz.")
            .When(x => x.CategoryId.HasValue);

        // ExpirationDate validation
        RuleFor(x => x.ExpirationDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Son kullanma tarihi bugünden ileri bir tarih olmalıdır.")
            .When(x => x.ExpirationDate.HasValue);
    }
}
