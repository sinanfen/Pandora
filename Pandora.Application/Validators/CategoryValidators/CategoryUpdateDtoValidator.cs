using FluentValidation;
using Pandora.Shared.DTOs.CategoryDTOs;

namespace Pandora.Application.Validators.CategoryValidators;

public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
{
    public CategoryUpdateDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ID cannot be empty.")
            .NotNull().WithMessage("ID is invalid.");

        RuleFor(x => x.UserId)
        .NotEmpty().WithMessage("User ID cannot be empty.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name cannot be empty.")
            .MinimumLength(3).WithMessage("Category name must be at least 3 characters long.");
    }
}
