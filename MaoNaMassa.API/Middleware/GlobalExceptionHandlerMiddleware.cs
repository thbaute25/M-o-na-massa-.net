using System.Net;
using System.Text.Json;
using MaoNaMassa.Domain.Exceptions;

namespace MaoNaMassa.API.Middleware;

/// <summary>
/// Middleware global para tratamento de exceções
/// </summary>
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger)
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
            _logger.LogError(ex, "Erro não tratado: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var problemDetails = new ProblemDetails();

        switch (exception)
        {
            case EntityNotFoundException notFoundEx:
                statusCode = HttpStatusCode.NotFound;
                problemDetails = new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    Title = "Recurso Não Encontrado",
                    Status = (int)statusCode,
                    Detail = notFoundEx.Message,
                    Instance = context.Request.Path
                };
                break;

            case DomainException domainEx:
                statusCode = HttpStatusCode.BadRequest;
                problemDetails = new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "Erro de Validação de Domínio",
                    Status = (int)statusCode,
                    Detail = domainEx.Message,
                    Instance = context.Request.Path
                };
                break;

            case ArgumentException argEx:
                statusCode = HttpStatusCode.BadRequest;
                problemDetails = new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "Argumento Inválido",
                    Status = (int)statusCode,
                    Detail = argEx.Message,
                    Instance = context.Request.Path
                };
                break;

            case UnauthorizedAccessException unauthorizedEx:
                statusCode = HttpStatusCode.Unauthorized;
                problemDetails = new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                    Title = "Não Autorizado",
                    Status = (int)statusCode,
                    Detail = unauthorizedEx.Message ?? "Você não tem permissão para acessar este recurso.",
                    Instance = context.Request.Path
                };
                break;

            default:
                problemDetails = new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    Title = "Erro Interno do Servidor",
                    Status = (int)statusCode,
                    Detail = "Ocorreu um erro inesperado. Tente novamente mais tarde.",
                    Instance = context.Request.Path
                };
                break;
        }

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/problem+json";

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(problemDetails, options);
        return context.Response.WriteAsync(json);
    }
}

/// <summary>
/// ProblemDetails conforme RFC 7807
/// </summary>
public class ProblemDetails
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public string Detail { get; set; } = string.Empty;
    public string Instance { get; set; } = string.Empty;
    public Dictionary<string, string[]>? Errors { get; set; }
}

