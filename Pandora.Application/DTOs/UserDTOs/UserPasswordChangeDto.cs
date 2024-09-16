namespace Pandora.Application.DTOs.UserDTOs;

public class UserPasswordChangeDto : BaseDto<Guid>
{
    public string CurrentPassword { get; set; }  // The user's current password (for verification)

    public string NewPassword { get; set; }  // The new password the user wants to set

    public string ConfirmNewPassword { get; set; }  // Confirmation of the new password

    // Optional: You can add a timestamp or security stamp to further ensure security if needed
}