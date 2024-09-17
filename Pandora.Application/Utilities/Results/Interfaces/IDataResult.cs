
namespace Pandora.Application.Utilities.Results.Interfaces;

public interface IDataResult<out T> : IResult
{
    public T Data { get; }
}