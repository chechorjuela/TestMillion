using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TestMillion.Application.Common.ResponseMessages;
using TestMillion.Application.Common.Results;

namespace ApplicationTask.Application.Common.Exceptions;

internal sealed class ValidationExceptionHandler(ILogger<BadRequestExceptionHandler> logger) : IExceptionHandler
{
  private readonly ILogger<BadRequestExceptionHandler> _logger = logger;

  public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
  {
    if (exception is not BadHttpRequestException badRequestException
        || badRequestException.Message != Messages.ValidationsErrorMessage)
    {
      return false;
    }

    _logger.LogError(
      badRequestException,
      "Exception occurred: {Message}",
      badRequestException.Message);

    var validations = ((ApplicationTask.Application.Common.Exceptions.ValidationException)exception).Errors?
      .Select(x => new ResultException()
      {
        ErrorMessage = x.ErrorMessage,
        PropertyName = x.PropertyName
      });

    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

    await httpContext.Response
      .WriteAsJsonAsync(validations, cancellationToken);

    return true;
  }
}