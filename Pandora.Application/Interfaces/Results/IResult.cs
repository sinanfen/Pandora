
namespace Pandora.Application.Interfaces.Results;

public interface IResult
{
    public ResultStatus ResultStatus { get; } //ResultStatus.Success
    public string Message { get; }
    public Exception Exception { get; }
    public IEnumerable<ValidationError> ValidationErrors { get; set; }
}
