namespace Pandora.Shared.DTOs.UserDTOs;

public class UserPasswordChangeDto : BaseDto<Guid>
{
    public string CurrentPassword { get; set; }  // The user's current password (for verification)

    public string NewPassword { get; set; }  // The new password the user wants to set

    public string ConfirmNewPassword { get; set; }  // Confirmation of the new password
    public DateTime? LastPasswordChangeDate { get; set; } // Last password change timestamp (optional)
}