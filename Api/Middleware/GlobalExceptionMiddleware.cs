using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Exceptions;

namespace Api.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        int statusCode;
        string message;
        object? details = null;

        switch (exception)
        {
            case ValidationException validationException:
                statusCode = (int)HttpStatusCode.BadRequest;
                message = validationException.Message;
                details = validationException.Errors;
                _logger.LogWarning(exception, "Error de validación capturado: {Message}", message);
                break;

            case NotFoundException notFoundException:
                statusCode = (int)HttpStatusCode.NotFound;
                message = notFoundException.Message;
                _logger.LogWarning(exception, "Recurso no encontrado capturado: {Message}", message);
                break;

            case KeyNotFoundException keyNotFoundException:
                statusCode = (int)HttpStatusCode.NotFound;
                message = keyNotFoundException.Message;
                _logger.LogWarning(exception, "Llave no encontrada capturada: {Message}", message);
                break;

            case UnauthorizedAccessException unauthorizedException:
                statusCode = (int)HttpStatusCode.Unauthorized;
                message = string.IsNullOrWhiteSpace(unauthorizedException.Message)
                    ? "Acceso no autorizado."
                    : unauthorizedException.Message;
                _logger.LogWarning(exception, "Acceso no autorizado capturado: {Message}", message);
                break;

            default:
                statusCode = (int)HttpStatusCode.InternalServerError;
                message = "Ha ocurrido un error interno en el servidor.";
                _logger.LogError(exception, "Error no controlado capturado por el middleware global: {Message}", exception.Message);
                break;
        }

        context.Response.StatusCode = statusCode;

        var response = new ErrorResponse
        {
            Status = statusCode,
            Message = message,
            Details = details
        };

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var json = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(json);
    }
}

public class ErrorResponse
{
    public int Status { get; set; }
    public string Message { get; set; } = string.Empty;
    public object? Details { get; set; }
}
