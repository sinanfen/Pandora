
namespace Pandora.Core.Domain.Interfaces;

public interface IQuery<T>
{
    IQueryable<T> Query();
}
