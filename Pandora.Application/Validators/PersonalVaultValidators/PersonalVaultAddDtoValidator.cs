using FluentValidation;
using Pandora.Shared.DTOs.PersonalVaultDTOs;

namespace Pandora.Application.Validators.PersonalVaultValidators;

public class PersonalVaultAddDtoValidator : AbstractValidator<PersonalVaultAddDto>
{
    public PersonalVaultAddDtoValidator()
    {
        // Title field validation - Simplified for flexible usage (notes, shopping lists, etc.)
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title cannot be empty.");

        // Content field validation - Simplified for flexible usage
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content cannot be empty.");

        // URL field validation - Made optional and only validate format when provided
        RuleFor(x => x.Url)
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute)).WithMessage("Please enter a valid URL.")
            .When(x => !string.IsNullOrEmpty(x.Url));

        // Tags field validation - Made optional, users don't have to provide tags
        // Removed mandatory tag requirement for flexibility

        // Category field (optional, but if provided, it should be valid)
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category ID is invalid.")
            .When(x => x.CategoryId.HasValue);

        // Optional expiration date validation
        RuleFor(x => x.ExpirationDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Expiration date must be a future date.")
            .When(x => x.ExpirationDate.HasValue);
    }
}
