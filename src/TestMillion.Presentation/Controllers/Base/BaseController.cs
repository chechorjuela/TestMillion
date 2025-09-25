using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Shared.Core;

namespace TestMillion.Presentation.Controllers.Base;

[ApiController]
[Route("/api/[controller]")]
[Produces("application/json")]
public abstract class BaseController : ControllerBase
{
  private IMediator? _mediator;

  protected IMediator Mediator => this._mediator ??= EngineContext.Current.Resolve<IMediator>();

  protected IMapper Mapper => EngineContext.Current.Resolve<IMapper>();

  protected ActionResult FromResult<T>(ResultResponse<T> result)
  {
    var response = new PagedResponse<T>();
    
    switch (result.StatusCode)
    {
      case ResultType.Ok:
      case ResultType.PartialOk:
        response = PagedResponse<T>.Success(result.Data, "Operation completed successfully", null);
        return Ok(response);
      
      case ResultType.Created:
        response = PagedResponse<T>.Success(result.Data, "Resource created successfully", null);
        return StatusCode(201, response);
      
      case ResultType.NotFound:
        response = PagedResponse<T>.Error("Resource not found", 404);
        return NotFound(response);
      
      case ResultType.Invalid:
      case ResultType.Unexpected:
        var errorMessage = result.Errors != null && result.Errors.Any() 
          ? string.Join(", ", result.Errors)
          : "An error occurred";
        response = PagedResponse<T>.Error(errorMessage, 400);
        return BadRequest(response);
      
      default:
        throw new Exception("Unhandled result.");
    }
  }

  protected ActionResult FromPagedResult<T>(PagedResponse<T> result) => result.Status switch
  {
    200 => this.Ok(result),
    _ => this.BadRequest(result.Message),
  };
}
