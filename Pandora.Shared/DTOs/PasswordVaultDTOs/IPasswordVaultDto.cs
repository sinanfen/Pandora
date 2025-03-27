
namespace Pandora.Shared.DTOs.PasswordVaultDTOs;

public interface IPasswordVaultDto
{
    string SiteName { get; set; }  // The site name, common to both Add and Update
    string UsernameOrEmail { get; set; }  // Username or email (AES-encrypted)
    string Password { get; set; }  // AES - password
    string Notes { get; set; }  // Optional notes (AES-encrypted)
}
