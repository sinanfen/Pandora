using FluentValidation;
using Pandora.Shared.DTOs.UserDTOs;

namespace Pandora.Application.Validators.UserValidators;

public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
{
    public UserUpdateDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ID cannot be empty.")
            .NotNull().WithMessage("ID is invalid.");

        // Username Validation
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username cannot be empty.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long.");

        // Email Validation
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .EmailAddress().WithMessage("Please enter a valid email address.");

        // Phone Number Validation
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number cannot be empty.");
    }
}
