using Microsoft.AspNetCore.Mvc;
using TestMillion.Application.Common.Models;
using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Properties.DTOs.Request;
using TestMillion.Application.Features.Properties.Cqrs.Commands.CreateProperty;
using TestMillion.Application.Features.Properties.Cqrs.Commands.DeleteProperty;
using TestMillion.Application.Features.Properties.Cqrs.Commands.UpdateProperty;
using TestMillion.Application.Features.Properties.Cqrs.Queries.GetProperties;
using TestMillion.Application.Features.Properties.Cqrs.Queries.GetByIdProperty;
using TestMillion.Application.Features.Properties.DTOs.Response;
using TestMillion.Application.Properties.DTOs.Request;
using TestMillion.Presentation.Controllers.Base;

namespace TestMillion.Presentation.Controllers;

public class PropertiesController : BaseController
{
  
  [HttpGet]
  [Produces(typeof(PagedResponse<List<PropertyResponseDto>>))]
  [ActionName(nameof(GetAllProperty))]
  public async Task<IActionResult> GetAllProperty([FromQuery] PaginationRequestDto pagination, [FromQuery] PropertyFilterDto filter = null)
  {
    var query = new GetPropertyAllQuery { Pagination = pagination, Filter = filter ?? new PropertyFilterDto() };
    var response = await this.Mediator.Send(query);
    return this.FromPagedResult(response);
  }
  
  [HttpPut("{id}")]
  [Produces(typeof(ResultResponse<PropertyResponseDto>))]
  [ActionName(nameof(UpdateProperty))]
  public async Task<IActionResult> UpdateProperty([FromRoute] string id, [FromBody] UpdatePropertyRequestDto request)
  {
    request.Id = id;

    var command = this.Mapper.Map<UpdatePropertyCommand>(request);
    var response = await this.Mediator.Send(command);
    return this.FromResult(response);
  }

  [HttpGet("{id}")]
  [Produces(typeof(ResultResponse<PropertyResponseDto>))]
  [ActionName(nameof(GetPropertyById))]
  public async Task<IActionResult> GetPropertyById(string id)
  {
    var query = new GetByIdPropertyQuery(id);
    var response = await this.Mediator.Send(query);
    return this.FromResult(response);
  }

  [HttpPost]
  [Produces(typeof(ResultResponse<PropertyResponseDto>))]
  [ActionName(nameof(CreateProperty))]
  public async Task<IActionResult> CreateProperty([FromBody] CreatePropertyRequestDto request)
  {
    var command = this.Mapper.Map<CreatePropertyCommand>(request);
    var response = await this.Mediator.Send(command);
    return this.FromResult(response);
  }

  [HttpDelete("{id}")]
  [Produces(typeof(ResultResponse<bool>))]
  [ActionName(nameof(DeleteProperty))]
  public async Task<IActionResult> DeleteProperty(string id)
  {
    var command = new DeletePropertyCommand(id);
    var response = await this.Mediator.Send(command);
    return this.FromResult(response);
  }
}