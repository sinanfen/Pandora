namespace Pandora.Core.Domain.Entities;

// Core/Domain/Entities/Entity.cs
public abstract class Entity<TId> : BaseEntity<TId>
{
    protected Entity() : base() { }

    protected Entity(TId id) : base(id) { }
}