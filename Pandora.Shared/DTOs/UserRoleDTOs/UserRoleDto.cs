
using Pandora.Shared.DTOs.RoleDTOs;
using Pandora.Shared.DTOs.UserDTOs;
using System.Data;

namespace Pandora.Shared.DTOs.UserRoleDTOs;

public class UserRoleDto
{
    public Guid UserId { get; set; }
    public UserDto User { get; set; }

    public Guid RoleId { get; set; }
    public RoleDto Role { get; set; }
}
