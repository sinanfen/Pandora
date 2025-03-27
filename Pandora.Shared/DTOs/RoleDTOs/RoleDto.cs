
using Pandora.Shared.DTOs.UserRoleDTOs;

namespace Pandora.Shared.DTOs.RoleDTOs;

public class RoleDto : BaseDto<Guid>
{
    public string Name { get; set; }          // Role name, e.g., "Admin", "User"
    public string NormalizedName { get; set; } // Used for case-insensitive checks

    public IList<UserRoleDto> UserRoles { get; set; } // Navigation property
}
