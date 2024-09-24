using FluentValidation;
using Pandora.Application.DTOs.CategoryDTOs;

namespace Pandora.Application.Validators.CategoryValidators;

public class CategoryAddDtoValidator : AbstractValidator<CategoryAddDto>
{
    public CategoryAddDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Kategori adı boş olamaz.")
            .MinimumLength(3).WithMessage("Kategori adı en az 3 karakter olmalıdır.");
    }
}
