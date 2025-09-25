using MediatR;
using Microsoft.AspNetCore.Mvc;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyTrace.Commands.CreatePropertyTrace;
using TestMillion.Application.Features.PropertyTrace.Cqrs.Queries.GetAllPropertyTrace;
using TestMillion.Application.Features.PropertyTrace.Cqrs.Queries.GetByPropertyTrace;
using TestMillion.Application.Features.PropertyTrace.DTOs.Request;
using TestMillion.Application.Features.PropertyTrace.DTOs.Resposnse;
using TestMillion.Presentation.Controllers.Base;

namespace TestMillion.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesTraceController : BaseController
{

  [HttpGet]
  [Produces(typeof(ResultResponse<List<PropertyTraceResponseDto>>))]
  [ActionName(nameof(GetAllPropertyTrace))]
  public async Task<IActionResult> GetAllPropertyTrace()
  {
    var query = new GetAllPropertyTraceQuery();
    var response = await this.Mediator.Send(query);
    return this.FromResult(response);
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
    request.Id = id;
    var command = this.Mapper.Map<UpdatePropertyTraceCommand>(request);
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