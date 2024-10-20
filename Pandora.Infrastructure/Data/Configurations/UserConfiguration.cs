using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pandora.Core.Domain.Constants.Enums;
using Pandora.Core.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

public class UserConfiguration : BaseEntityConfiguration<User, Guid>
{
    private static readonly Guid adminId = Guid.NewGuid();
    private static readonly Guid userId = Guid.NewGuid();

    public static Guid GetAdminId() => adminId;
    public static Guid GetUserId() => userId;

    public override void Configure(EntityTypeBuilder<User> builder)
    {
        // Call the base configuration for common fields
        base.Configure(builder);

        // Table name and primary key
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);

        // Properties
        builder.Property(u => u.Username)
               .IsRequired()
               .HasMaxLength(256);

        builder.Property(u => u.NormalizedUsername)
               .IsRequired()
               .HasMaxLength(256);

        builder.Property(u => u.Email)
               .IsRequired()
               .HasMaxLength(256);

        builder.Property(u => u.NormalizedEmail)
               .IsRequired()
               .HasMaxLength(256);

        builder.Property(u => u.PasswordHash)
               .IsRequired();

        // Optional fields
        builder.Property(u => u.PhoneNumber)
               .HasMaxLength(20);

        builder.Property(u => u.LastLoginDate)
               .IsRequired();

        // Uniqueness constraints
        builder.HasIndex(u => u.NormalizedUsername)
               .IsUnique();

        builder.HasIndex(u => u.NormalizedEmail)
               .IsUnique();


        var adminUser = new User
        {
            Id = adminId,
            Username = "admin",
            NormalizedUsername = "ADMIN",
            PhoneNumber = "1234567890",
            SecurityStamp = Guid.NewGuid().ToString(),
            FirstName = "Admin",
            LastName = "Admin",
            Email = "admin@example.com",
            NormalizedEmail = "ADMIN@EXAMPLE.COM",
            PasswordHash = HashPassword("AdminPassword123"),
            LastLoginDate = DateTime.UtcNow,
            EmailConfirmed = true
        };

        var regularUser = new User
        {
            Id = userId,
            Username = "user",
            NormalizedUsername = "USER",
            PhoneNumber = "1234567890",
            SecurityStamp = Guid.NewGuid().ToString(),
            FirstName = "User",
            LastName = "User",
            Email = "user@example.com",
            NormalizedEmail = "USER@EXAMPLE.COM",
            PasswordHash = HashPassword("UserPassword123"),
            LastLoginDate = DateTime.UtcNow,
            EmailConfirmed = true
        };

        builder.HasData(adminUser, regularUser);
    }

    private string HashPassword(string password)
    {
        using var sha = SHA512.Create();
        var hashedBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }
}

// Core/Domain/Configurations/RoleConfiguration.cs
public class RoleConfiguration : BaseEntityConfiguration<Role, Guid>
{

    private static readonly Guid adminRoleId = Guid.NewGuid();
    private static readonly Guid userRoleId = Guid.NewGuid();

    public static Guid GetAdminRoleId() => adminRoleId;
    public static Guid GetUserRoleId() => userRoleId;

    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        // Seed roles
        var roles = new List<Role>
        {
            new Role { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
            new Role { Id = userRoleId, Name = "User", NormalizedName = "USER" }
        };

        base.Configure(builder);

        builder.ToTable("Roles");

        builder.Property(r => r.Name)
               .IsRequired()
               .HasMaxLength(256);

        builder.HasMany(r => r.UserRoles)
               .WithOne(ur => ur.Role)
               .HasForeignKey(ur => ur.RoleId)
               .IsRequired();

        builder.HasData(roles);
    }
}

// Core/Domain/Configurations/UserRoleConfiguration.cs
public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");

        builder.HasKey(x => new { x.UserId, x.RoleId });

        var adminUserId = UserConfiguration.GetAdminId();
        var regularUserId = UserConfiguration.GetUserId();

        var adminRoleId = RoleConfiguration.GetAdminRoleId();
        var userRoleId = RoleConfiguration.GetUserRoleId();

        var userRoles = new List<UserRole>
        {
            new UserRole { UserId = adminUserId, RoleId = adminRoleId },
            new UserRole { UserId = regularUserId, RoleId = userRoleId }
        };

        builder.HasData(userRoles);
    }
}
