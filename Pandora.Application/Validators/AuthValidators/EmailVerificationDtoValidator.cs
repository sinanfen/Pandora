using FluentValidation;
using Pandora.Shared.DTOs.AuthDTOs;

namespace Pandora.Application.Validators.AuthValidators;

public class EmailVerificationDtoValidator : AbstractValidator<EmailVerificationDto>
{
    public EmailVerificationDtoValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Verification token is required")
            .Length(1, 256)
            .WithMessage("Token must be between 1 and 256 characters")
            .Matches("^[A-Za-z0-9+/=]+$")
            .WithMessage("Token contains invalid characters");
    }
}

public class ResendEmailVerificationDtoValidator : AbstractValidator<ResendEmailVerificationDto>
{
    public ResendEmailVerificationDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Valid email address is required")
            .Length(5, 254)
            .WithMessage("Email must be between 5 and 254 characters");
    }
}

public class ChangeEmailDtoValidator : AbstractValidator<ChangeEmailDto>
{
    public ChangeEmailDtoValidator()
    {
        RuleFor(x => x.NewEmail)
            .NotEmpty()
            .WithMessage("New email is required")
            .EmailAddress()
            .WithMessage("Valid email address is required")
            .Length(5, 254)
            .WithMessage("Email must be between 5 and 254 characters");

        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .WithMessage("Current password is required")
            .Length(6, 100)
            .WithMessage("Password must be between 6 and 100 characters");
    }
} 