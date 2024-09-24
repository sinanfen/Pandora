using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pandora.Core.Domain.Entities;

public class PersonalVaultConfiguration : BaseEntityConfiguration<PersonalVault, Guid>
{
    public void Configure(EntityTypeBuilder<PersonalVault> builder)
    {
        // Call the base configuration for common fields
        base.Configure(builder);

        // Table name and primary key
        builder.ToTable("PersonalVaults");
        builder.HasKey(pb => pb.Id);

        // Properties
        builder.Property(pb => pb.Title)
               .IsRequired()
               .HasMaxLength(256);

        builder.Property(pb => pb.EncryptedContent)
               .IsRequired(); // Content is AES-encrypted, no max length specified as it's variable length

        builder.Property(pb => pb.Summary)
               .HasMaxLength(512); // Optional summary, limit length

        builder.Property(pb => pb.EncryptedUrl)
               .HasMaxLength(512); // Optional, limit URL length

        builder.Property(pb => pb.EncryptedMediaFile)
               .HasMaxLength(512); // Optional, limit media file path length

        builder.Property(pb => pb.IsFavorite)
               .IsRequired(); // Flag to mark if it's a favorite    

        builder.Property(pb => pb.LastModifiedDate)
               .IsRequired(false); // Last modified date is optional

        builder.Property(pb => pb.ExpirationDate)
               .IsRequired(false); // Expiration date is optional

        // Relationships
        builder.HasOne(pb => pb.User)
               .WithMany(u => u.PersonalVaults)
               .HasForeignKey(pb => pb.UserId);

        builder.HasOne(pb => pb.Category)
               .WithMany(c => c.PersonalVaults)
               .HasForeignKey(pb => pb.CategoryId)
               .OnDelete(DeleteBehavior.SetNull); // Optional category, set null on delete

        // Tags are stored as a collection
        builder.Property(pb => pb.Tags)
               .HasConversion(
                    tags => string.Join(',', tags), // Store tags as comma-separated string
                    tags => tags.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()); // Convert back to List<string>

        builder.Property(b => b.CreatedDate)
        .HasColumnName("CreatedDate")
        .HasDefaultValueSql("CURRENT_TIMESTAMP")
        .IsRequired();
            builder.Property(b => b.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(b => b.DeletedDate).HasColumnName("DeletedDate");

        builder.HasQueryFilter(b => !b.DeletedDate.HasValue);
    }
}
