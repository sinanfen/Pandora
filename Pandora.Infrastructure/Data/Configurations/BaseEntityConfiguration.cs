using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pandora.Core.Domain.Entities;

public abstract class BaseEntityConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity<TId>
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        // Configure common properties
        builder.Property(b => b.CreatedDate)
               .HasColumnName("CreatedDate")
               .HasDefaultValueSql("CURRENT_TIMESTAMP") // Set default value to current timestamp
               .IsRequired();

        builder.Property(b => b.UpdatedDate)
               .HasColumnName("UpdatedDate");

        builder.Property(b => b.DeletedDate)
               .HasColumnName("DeletedDate");

        // Add a query filter to automatically exclude soft-deleted entries
        builder.HasQueryFilter(b => !b.DeletedDate.HasValue);
    }
}