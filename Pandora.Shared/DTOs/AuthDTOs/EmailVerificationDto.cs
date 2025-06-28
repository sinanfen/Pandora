using System.ComponentModel.DataAnnotations;

namespace Pandora.Shared.DTOs.AuthDTOs;

public class EmailVerificationDto
{
    [Required(ErrorMessage = "Email verification token is required")]
    [StringLength(256, ErrorMessage = "Token cannot exceed 256 characters")]
    public string Token { get; set; } = string.Empty;
}

public class ResendEmailVerificationDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Valid email address is required")]
    [StringLength(254, ErrorMessage = "Email cannot exceed 254 characters")]
    public string Email { get; set; } = string.Empty;
}

public class EmailVerificationResponseDto
{
    public bool IsVerified { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime? VerifiedAt { get; set; }
}

public class ChangeEmailDto
{
    [Required(ErrorMessage = "New email is required")]
    [EmailAddress(ErrorMessage = "Valid email address is required")]
    [StringLength(254, ErrorMessage = "Email cannot exceed 254 characters")]
    public string NewEmail { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Current password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6-100 characters")]
    public string CurrentPassword { get; set; } = string.Empty;
} 