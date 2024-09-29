
namespace Pandora.Application.DTOs.PasswordVaultDTOs;

public interface IPasswordVaultDto
{
    string SiteName { get; set; }  // The site name, common to both Add and Update
    string UsernameOrEmail { get; set; }  // Username or email (AES-encrypted)
    string Notes { get; set; }  // Optional notes (AES-encrypted)
}
