using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pandora.Core.Domain.Entities;

public class PasswordVaultConfiguration : BaseEntityConfiguration<PasswordVault, Guid>
{
    public override void Configure(EntityTypeBuilder<PasswordVault> builder)
    {
        // Call the base configuration for common fields
        base.Configure(builder);

        // Configure the table and primary key
        builder.ToTable("PasswordVaults");
        builder.HasKey(pv => pv.Id);

        // Configure properties
        builder.Property(pv => pv.SecureSiteName)
               .IsRequired()
               .HasMaxLength(256); // Limit site name length

        builder.Property(pv => pv.SecureUsernameOrEmail)
               .IsRequired(); // AES-encrypted field

        builder.Property(pv => pv.PasswordHash)
               .IsRequired(); // Hashed password (irreversible)

        builder.Property(pv => pv.SecureNotes)
               .HasMaxLength(512); // Optional notes (encrypted)

        builder.Property(pv => pv.LastPasswordChangeDate)
               .IsRequired(false); // Optional field

        // Relationships
        builder.HasOne(pv => pv.User)
               .WithMany(u => u.PasswordVaults)
               .HasForeignKey(pv => pv.UserId);

        builder.HasOne(pv => pv.Category)
               .WithMany(c => c.PasswordVaults)
               .HasForeignKey(pv => pv.CategoryId)
               .OnDelete(DeleteBehavior.SetNull); // Optional category
    }
}
