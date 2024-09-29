﻿using FluentValidation;
using Pandora.Application.DTOs.PasswordVaultDTOs;

namespace Pandora.Application.Validators.PasswordVaultValidators;

public class PasswordVaultAddDtoValidator : AbstractValidator<PasswordVaultAddDto>
{
    public PasswordVaultAddDtoValidator()
    {
        // UserId field validation
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Kullanıcı kimliği boş olamaz.");

        // SiteName field validation
        RuleFor(x => x.SiteName)
            .NotEmpty().WithMessage("Site adı boş olamaz.")
            .MinimumLength(3).WithMessage("Site adı en az 3 karakter olmalıdır.");

        // UsernameOrEmail field validation
        RuleFor(x => x.UsernameOrEmail)
            .NotEmpty().WithMessage("Kullanıcı adı veya email boş olamaz.")
            .MinimumLength(5).WithMessage("Kullanıcı adı veya email en az 5 karakter olmalıdır.");

        // Password field validation
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre boş olamaz.")
            .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");

        //// PasswordExpirationDate validation (optional)
        //RuleFor(x => x.PasswordExpirationDate)
        //    .GreaterThan(DateTime.UtcNow).WithMessage("Şifre son kullanma tarihi bugünden ileri bir tarih olmalıdır.")
        //    .When(x => x.PasswordExpirationDate.HasValue);

        // CategoryId validation (optional)
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Geçerli bir kategori seçilmelidir.")
            .When(x => x.CategoryId.HasValue);
    }
}
