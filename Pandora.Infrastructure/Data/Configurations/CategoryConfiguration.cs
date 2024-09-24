using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pandora.Core.Domain.Entities;

public class CategoryConfiguration : BaseEntityConfiguration<Category, Guid>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        // Call the base configuration for common fields
        base.Configure(builder);

        // Configure table and primary key
        builder.ToTable("Categories");
        builder.HasKey(c => c.Id);

        // Configure the properties
        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(256); // Limit category name length to 256 characters

        builder.HasIndex(c => c.Name).IsUnique();

        builder.Property(c => c.Description)
               .HasMaxLength(512); // Optional description with a max length of 512 characters

        // Relationships
        builder.HasMany(c => c.PasswordVaults)
               .WithOne(pv => pv.Category)
               .HasForeignKey(pv => pv.CategoryId)
               .OnDelete(DeleteBehavior.SetNull); // Optional category for PasswordVaults

        builder.HasMany(c => c.PersonalVaults)
               .WithOne(pb => pb.Category)
               .HasForeignKey(pb => pb.CategoryId)
               .OnDelete(DeleteBehavior.SetNull); // Optional category for PersonalVaults
    }
}
