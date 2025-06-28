namespace Pandora.API.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // Proceed to next middleware
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var path = context.Request.Path;
        var traceId = context.TraceIdentifier;

        _logger.LogError(exception, "Unhandled exception occurred. Path: {Path} | TraceId: {TraceId}", path, traceId);

        context.Response.ContentType = "application/json";

        object errorResponse;
        int statusCode;

        switch (exception)
        {
            case UnauthorizedAccessException:
                statusCode = StatusCodes.Status401Unauthorized;
                errorResponse = new
                {
                    status = statusCode,
                    message = "Unauthorized access.",
                    traceId = traceId
                };
                break;
            case ArgumentNullException or ArgumentException:
                statusCode = StatusCodes.Status400BadRequest;
                errorResponse = new
                {
                    status = statusCode,
                    message = "Invalid request parameter.",
                    traceId = traceId,
                    detail = _env.IsDevelopment() ? exception.Message : null
                };
                break;
            case NotImplementedException:
                statusCode = StatusCodes.Status501NotImplemented;
                errorResponse = new
                {
                    status = statusCode,
                    message = "This feature is not implemented yet.",
                    traceId = traceId,
                    detail = _env.IsDevelopment() ? exception.Message : null
                };
                break;
            case KeyNotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                errorResponse = new
                {
                    status = statusCode,
                    message = "Record not found.",
                    traceId = traceId,
                    detail = _env.IsDevelopment() ? exception.Message : null
                };
                break;
            case InvalidOperationException:
                statusCode = StatusCodes.Status409Conflict;
                errorResponse = new
                {
                    status = statusCode,
                    message = "Invalid operation.",
                    traceId = traceId,
                    detail = _env.IsDevelopment() ? exception.Message : null
                };
                break;
            case OperationCanceledException:
                statusCode = StatusCodes.Status408RequestTimeout;
                errorResponse = new
                {
                    status = statusCode,
                    message = "Request timed out.",
                    traceId = traceId,
                    detail = _env.IsDevelopment() ? exception.Message : null
                };
                break;
            default:
                statusCode = StatusCodes.Status500InternalServerError;
                errorResponse = new
                {
                    status = statusCode,
                    message = "An unexpected error occurred.",
                    traceId = traceId,
                    detail = _env.IsDevelopment() ? exception.Message : null
                };
                break;
        }

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(errorResponse);
    }
}
