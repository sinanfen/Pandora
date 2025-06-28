using FluentValidation;
using Pandora.Shared.DTOs.AuthDTOs;

namespace Pandora.Application.Validators.AuthValidators;

public class RefreshTokenDtoValidator : AbstractValidator<RefreshTokenDto>
{
    public RefreshTokenDtoValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required")
            .MinimumLength(50).WithMessage("Invalid refresh token format");
    }
}

public class RevokeTokenDtoValidator : AbstractValidator<RevokeTokenDto>
{
    public RevokeTokenDtoValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required")
            .MinimumLength(50).WithMessage("Invalid refresh token format");

        RuleFor(x => x.Reason)
            .MaximumLength(200).WithMessage("Reason cannot exceed 200 characters");
    }
} 