namespace Pandora.Core.Domain.Entities;

// Core/Domain/Entities/UserRole.cs
public class UserRole 
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid RoleId { get; set; }
    public Role Role { get; set; }
}
