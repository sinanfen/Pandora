namespace Pandora.Core.Domain.Entities;

// Core/Domain/Entities/Category.cs
public class Category : Entity<Guid>
{
    public string Name { get; set; }  // Category name, e.g., Social Media, Work
    public string Description { get; set; }

    // Navigation properties to link with vaults and Pandora's boxes
    public ICollection<PasswordVault> PasswordVaults { get; set; }
    public ICollection<PersonalVault> PersonalVaults { get; set; }

    public Category() : base(Guid.NewGuid())
    {
        PasswordVaults = new List<PasswordVault>();
        PersonalVaults = new List<PersonalVault>();
    }
}
