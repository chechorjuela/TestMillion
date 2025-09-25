using MediatR;
using Microsoft.AspNetCore.Mvc;
using TestMillion.Application.Common.Models;
using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Owners.Cqrs.Commands.CreateOwner;
using TestMillion.Application.Features.Owners.Cqrs.Commands.DeleteOwner;
using TestMillion.Application.Features.Owners.Cqrs.Commands.UpdateOwner;
using TestMillion.Application.Features.Owners.DTOs.Request;
using TestMillion.Application.Features.Owners.DTOs.Response;
using TestMillion.Application.Features.Owners.Cqrs.Queries.GetAllOwner;
using TestMillion.Application.Features.Owners.Cqrs.Queries.GetOwnerById;
using TestMillion.Presentation.Controllers.Base;

namespace TestMillion.Presentation.Controllers;

public class OwnerController : BaseController
{
  [HttpGet]
  [Produces(typeof(PagedResponse<List<OwnerResponseDto>>))]
  [ActionName(nameof(GetAllOwner))]
  public async Task<IActionResult> GetAllOwner([FromQuery] PaginationRequestDto pagination, [FromQuery] FilterRequestDto filter = null)
  {
    var query = new GetAllOwnerQuery { Pagination = pagination, Filter = filter ?? new FilterRequestDto() };
    var response = await this.Mediator.Send(query);
    return this.FromPagedResult(response);
  }
  
  [HttpGet("{id}")]
  [Produces(typeof(ResultResponse<OwnerResponseDto>))]
  [ActionName(nameof(GetOwnerById))]
  public async Task<IActionResult> GetOwnerById([FromRoute] string id)
  {
    var query = new GetByIdOwnerQuery(id);
    var response = await this.Mediator.Send(query);
    return FromResult(response);
  }
  
  [HttpPost]
  [Produces(typeof(ResultResponse<OwnerResponseDto>))]
  [ActionName(nameof(CreateOwner))]
  public async Task<IActionResult> CreateOwner(CreateOwnerRequestDto request)
  {
    var command = this.Mapper.Map<CreateOwnerCommand>(request);
    var response = await this.Mediator.Send(command);
    return this.FromResult(response);
  }
  
  [HttpPut("{id}")]
  [Produces(typeof(ResultResponse<OwnerResponseDto>))]
  [ActionName(nameof(UpdateOwner))]
  public async Task<IActionResult> UpdateOwner([FromRoute] string id, [FromBody] UpdateOwnerRequestDto request)
  {
    if (string.IsNullOrEmpty(id))
    {
        return BadRequest("Id cannot be empty");
    }

    request.Id = id;
    var command = this.Mapper.Map<UpdateOwnerCommand>(request);
    
    if (command == null)
    {
        return BadRequest("Invalid request data");
    }

    var response = await this.Mediator.Send(command);
    return this.FromResult(response);
  }

  [HttpDelete("{id}")]
  [Produces(typeof(ResultResponse<bool>))]
  [ActionName(nameof(DeleteOwner))]
  public async Task<IActionResult> DeleteOwner([FromRoute] string id)
  {
    var command = new DeleteOwnerCommand { Id = id };
    var response = await this.Mediator.Send(command);
    return this.FromResult(response);
  }
}