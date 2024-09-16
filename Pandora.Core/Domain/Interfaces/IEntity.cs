namespace Pandora.Core.Domain.Interfaces;

// Core/Domain/Interfaces/IEntity.cs
public interface IEntity<T>
{
    T Id { get; set; }
}
