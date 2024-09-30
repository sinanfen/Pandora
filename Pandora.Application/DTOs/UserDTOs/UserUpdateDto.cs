namespace Pandora.Application.DTOs.UserDTOs;

public class UserUpdateDto : BaseDto<Guid>
{
    // Common fields for all users
    public string Username { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool TwoFactorEnabled { get; set; }

    // Corporate-specific fields
    public string CompanyName { get; set; }  // Updatable for corporate users
    public string TaxNumber { get; set; }    // Updatable for corporate users

    // Individual-specific fields
    public string FirstName { get; set; }    // Updatable for individual users
    public string LastName { get; set; }     // Updatable for individual users

    // Timestamps for auditing
    public DateTime? LastLoginDate { get; set; } // Last login timestamp
}
