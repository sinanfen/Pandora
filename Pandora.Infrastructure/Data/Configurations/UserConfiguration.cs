using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pandora.Core.Domain.Constants.Enums;
using Pandora.Core.Domain.Entities;

public class UserConfiguration : BaseEntityConfiguration<User, Guid>
{
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

        // Discriminator for Individual and Corporate Users
        builder.HasDiscriminator<UserType>("UserType")
               .HasValue<IndividualUser>(UserType.Individual)
               .HasValue<CorporateUser>(UserType.Corporate);
    }
}
