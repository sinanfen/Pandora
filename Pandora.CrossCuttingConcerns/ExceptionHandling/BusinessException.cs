
namespace Pandora.CrossCuttingConcerns.ExceptionHandling;

public class BusinessException : Exception
{
    public BusinessException(string message) : base(message)
    {
    }
}
