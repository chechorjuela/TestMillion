using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TestMillion.Application.Common.Response;

namespace TestMillion.Application.Common.Exceptions.Validation;

public class ValidationExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ValidationExceptionHandler> _logger;
    private readonly IHostEnvironment _environment;

    public ValidationExceptionHandler(
        ILogger<ValidationExceptionHandler> logger,
        IHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
        {
            return false;
        }

        _logger.LogWarning(
            "Validation error occurred for {Path}: {Message}",
            httpContext.Request.Path,
            validationException.Message);

        var response = new ValidationErrorResponse
        {
            Status = StatusCodes.Status400BadRequest,
            Message = "Validation failed",
            Errors = validationException.Errors
        };

        if (_environment.IsDevelopment())
        {
            response.Debug = new DebugInfo
            {
                ExceptionType = validationException.GetType().FullName ?? string.Empty,
                StackTrace = validationException.StackTrace ?? string.Empty,
                Path = httpContext.Request.Path.Value ?? string.Empty,
                Endpoint = httpContext.GetEndpoint()?.DisplayName ?? string.Empty
            };
        }

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}