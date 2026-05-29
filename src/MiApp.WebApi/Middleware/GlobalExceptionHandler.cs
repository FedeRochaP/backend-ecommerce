using FluentValidation;
using MiApp.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MiApp.WebApi.Middleware;

/// <summary>
/// Intercepta todas las excepciones no manejadas y devuelve una respuesta JSON uniforme.
/// Registrado como IExceptionHandler (ASP.NET Core 8).
/// </summary>
public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) => _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is ValidationException validationEx)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            var errors = validationEx.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray());

            await httpContext.Response.WriteAsJsonAsync(
                new ValidationProblemDetails(errors)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title  = "Error de validación."
                },
                cancellationToken);

            return true;
        }

        var (statusCode, message) = exception switch
        {
            DomainException ex           => (StatusCodes.Status400BadRequest,  ex.Message),
            ArgumentException ex         => (StatusCodes.Status400BadRequest,  ex.Message),
            UnauthorizedAccessException ex => (StatusCodes.Status401Unauthorized, ex.Message),
            _                            => (StatusCodes.Status500InternalServerError, "Ocurrió un error inesperado.")
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
            _logger.LogError(exception, "Excepción no controlada");

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = statusCode,
                Title  = message
            },
            cancellationToken);

        return true;
    }
}
