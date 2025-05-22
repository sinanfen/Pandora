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
                    message = "Yetkisiz erişim.",
                    traceId = traceId
                };
                break;
            case ArgumentNullException or ArgumentException:
                statusCode = StatusCodes.Status400BadRequest;
                errorResponse = new
                {
                    status = statusCode,
                    message = "Geçersiz istek parametresi.",
                    traceId = traceId,
                    detail = _env.IsDevelopment() ? exception.Message : null
                };
                break;
            case NotImplementedException:
                statusCode = StatusCodes.Status501NotImplemented;
                errorResponse = new
                {
                    status = statusCode,
                    message = "Bu özellik henüz uygulanmadı.",
                    traceId = traceId,
                    detail = _env.IsDevelopment() ? exception.Message : null
                };
                break;
            case KeyNotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                errorResponse = new
                {
                    status = statusCode,
                    message = "Kayıt bulunamadı.",
                    traceId = traceId,
                    detail = _env.IsDevelopment() ? exception.Message : null
                };
                break;
            case InvalidOperationException:
                statusCode = StatusCodes.Status409Conflict;
                errorResponse = new
                {
                    status = statusCode,
                    message = "Geçersiz işlem.",
                    traceId = traceId,
                    detail = _env.IsDevelopment() ? exception.Message : null
                };
                break;
            case OperationCanceledException:
                statusCode = StatusCodes.Status408RequestTimeout;
                errorResponse = new
                {
                    status = statusCode,
                    message = "İstek zaman aşımına uğradı.",
                    traceId = traceId,
                    detail = _env.IsDevelopment() ? exception.Message : null
                };
                break;
            default:
                statusCode = StatusCodes.Status500InternalServerError;
                errorResponse = new
                {
                    status = statusCode,
                    message = "Beklenmeyen bir hata oluştu.",
                    traceId = traceId,
                    detail = _env.IsDevelopment() ? exception.Message : null
                };
                break;
        }

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(errorResponse);
    }
}
