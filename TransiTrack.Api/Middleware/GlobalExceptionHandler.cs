using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TransiTrack.Api.Middleware;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var statusCode = exception is BadHttpRequestException badRequest
            ? badRequest.StatusCode
            : StatusCodes.Status500InternalServerError;
        if (statusCode == StatusCodes.Status500InternalServerError)
            logger.LogError(exception, "Unhandled API exception for {Path}", httpContext.Request.Path);
        else
            logger.LogWarning(exception, "Invalid API request for {Path}", httpContext.Request.Path);

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = statusCode,
                Title = statusCode == StatusCodes.Status400BadRequest
                    ? "Requête invalide."
                    : "Une erreur interne est survenue.",
                Detail = statusCode == StatusCodes.Status400BadRequest
                    ? "Le corps de la requête est invalide."
                    : null,
                Instance = httpContext.Request.Path
            },
            cancellationToken);

        return true;
    }
}
