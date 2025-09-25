using MediatR;
using Microsoft.AspNetCore.Mvc;
using TestMillion.Application.Common.Models;
using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyTrace.Commands.CreatePropertyTrace;
using TestMillion.Application.Features.PropertyTrace.Commands.DeletePropertyTrace;
using TestMillion.Application.Features.PropertyTrace.Commands.UpdatePropertyTrace;
using TestMillion.Application.Features.PropertyTrace.Cqrs.Queries.GetAllPropertyTrace;
using TestMillion.Application.Features.PropertyTrace.Cqrs.Queries.GetByPropertyTrace;
using TestMillion.Application.Features.PropertyTrace.DTOs.Request;
using TestMillion.Application.Features.PropertyTrace.DTOs.Response;
using TestMillion.Presentation.Controllers.Base;

namespace TestMillion.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesTraceController : BaseController
{

  [HttpGet]
  [Produces(typeof(PagedResponse<List<PropertyTraceResponseDto>>))]
  [ActionName(nameof(GetAllPropertyTrace))]
  public async Task<IActionResult> GetAllPropertyTrace([FromQuery] PaginationRequestDto pagination, [FromQuery] FilterRequestDto filter = null)
  {
    var query = new GetAllPropertyTraceQuery { Pagination = pagination, Filter = filter ?? new FilterRequestDto() };
    var response = await this.Mediator.Send(query);
    return this.FromPagedResult(response);
  }
  
  [HttpGet("{id}")]
  [Produces(typeof(ResultResponse<PropertyTraceResponseDto>))]
  [ActionName(nameof(GetPropertyTraceById))]
  public async Task<IActionResult> GetPropertyTraceById([FromRoute] string id)
  {
    var query = new GetByPropertyTraceQuery(id);
    var response = await this.Mediator.Send(query);
    return FromResult(response);
  }
  
  [HttpPost]
  [Produces(typeof(ResultResponse<PropertyTraceResponseDto>))]
  [ActionName(nameof(CreatePropertyTrace))]
  public async Task<IActionResult> CreatePropertyTrace(CreatePropertyTraceRequestDto request)
  {
    var command = this.Mapper.Map<CreatePropertyTraceCommand>(request);
    var response = await this.Mediator.Send(command);
    return this.FromResult(response);
  }
  
  [HttpPut("{id}")]
  [Produces(typeof(ResultResponse<PropertyTraceResponseDto>))]
  [ActionName(nameof(UpdatePropertyTrace))]
  public async Task<IActionResult> UpdatePropertyTrace([FromRoute] string id, [FromBody] UpdatePropertyTraceRequestDto request)
  {
    var command = this.Mapper.Map<UpdatePropertyTraceCommand>(request);
    command.Id = id;
    var response = await this.Mediator.Send(command);
    return this.FromResult(response);
  }

  [HttpDelete("{id}")]
  [Produces(typeof(ResultResponse<bool>))]
  [ActionName(nameof(DeletePropertyTrace))]
  public async Task<IActionResult> DeletePropertyTrace([FromRoute] string id)
  {
    var command = new DeletePropertyTraceCommand { Id = id };
    var response = await this.Mediator.Send(command);
    return this.FromResult(response);
  }
}