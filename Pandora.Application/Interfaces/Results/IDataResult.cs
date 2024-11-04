namespace Pandora.Application.Interfaces.Results;

public interface IDataResult<out T> : IResult
{
    public T Data { get; }
}