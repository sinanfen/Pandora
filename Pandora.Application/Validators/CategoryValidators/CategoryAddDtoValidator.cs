using FluentValidation;
using Pandora.Shared.DTOs.CategoryDTOs;

namespace Pandora.Application.Validators.CategoryValidators;

public class CategoryAddDtoValidator : AbstractValidator<CategoryAddDto>
{
    public CategoryAddDtoValidator()
    {
        RuleFor(x => x.UserId)
        .NotEmpty().WithMessage("User ID cannot be empty.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name cannot be empty.")
            .MinimumLength(3).WithMessage("Category name must be at least 3 characters long.");
    }
}
