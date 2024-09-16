using Pandora.Core.Domain.Interfaces;

namespace Pandora.Core.Domain.Entities;

// Core/Domain/Entities/BaseEntity.cs
public abstract class BaseEntity<TId> : IEntity<TId>, IEntityTimestamps
{
    public TId Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }

    protected BaseEntity()
    {
        Id = default(TId);
        CreatedDate = DateTime.UtcNow;
    }

    protected BaseEntity(TId id)
    {
        Id = id;
        CreatedDate = DateTime.UtcNow;
    }

    // Optional: Add methods for marking the entity as updated or deleted
    public void MarkAsUpdated()
    {
        UpdatedDate = DateTime.UtcNow;
    }

    public void MarkAsDeleted()
    {
        DeletedDate = DateTime.UtcNow;
    }
}