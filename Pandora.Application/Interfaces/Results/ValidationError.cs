namespace Pandora.Application.Interfaces.Results;

public class ValidationError
{
    public string PropertyName { get; set; }
    public string Message { get; set; }
}
