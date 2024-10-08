using FluentValidation;
using Pandora.Shared.DTOs.CategoryDTOs;

namespace Pandora.Application.Validators.CategoryValidators;

public class CategoryAddDtoValidator : AbstractValidator<CategoryAddDto>
{
    public CategoryAddDtoValidator()
    {
        RuleFor(x => x.UserId)
        .NotEmpty().WithMessage("Kullanıcı kimliği boş olamaz.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Kategori adı boş olamaz.")
            .MinimumLength(3).WithMessage("Kategori adı en az 3 karakter olmalıdır.");
    }
}
