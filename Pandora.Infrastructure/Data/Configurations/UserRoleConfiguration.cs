using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pandora.Core.Domain.Entities;

namespace Pandora.Infrastructure.Data.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        // Composite Primary Key
        builder.HasKey(ur => new { ur.UserId, ur.RoleId });
        
        // Properties
        builder.Property(ur => ur.UserId)
               .IsRequired();

        builder.Property(ur => ur.RoleId)
               .IsRequired();

        // Relationships
        builder.HasOne(ur => ur.User)
               .WithMany(u => u.UserRoles)
               .HasForeignKey(ur => ur.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ur => ur.Role)
               .WithMany(r => r.UserRoles)
               .HasForeignKey(ur => ur.RoleId)
               .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(ur => ur.UserId)
               .HasDatabaseName("IX_UserRoles_UserId");

        builder.HasIndex(ur => ur.RoleId)
               .HasDatabaseName("IX_UserRoles_RoleId");

        // Table name
        builder.ToTable("UserRoles");
    }
} 