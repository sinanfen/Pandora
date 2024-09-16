namespace Pandora.Core.Domain.Interfaces;

// Core/Domain/Interfaces/IEntityTimestamps.cs
public interface IEntityTimestamps
{
    DateTime CreatedDate { get; set; }
    DateTime? UpdatedDate { get; set; }
    DateTime? DeletedDate { get; set; }
}
