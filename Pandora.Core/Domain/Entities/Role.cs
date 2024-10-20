namespace Pandora.Core.Domain.Entities;

// Core/Domain/Entities/Role.cs
public class Role : Entity<Guid>
{
    public string Name { get; set; }          // Role name, e.g., "Admin", "User"
    public string NormalizedName { get; set; } // Used for case-insensitive checks

    public ICollection<UserRole> UserRoles { get; set; } // Navigation property

    public Role() : base(Guid.NewGuid())
    {
        UserRoles = new List<UserRole>();
    }
}
