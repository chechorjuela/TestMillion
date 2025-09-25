using Microsoft.AspNetCore.Mvc;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Owners.Cqrs.Queries.GetOwnerById;
using TestMillion.Application.Features.Properties.Cqrs.Commands.CreateProperty;
using TestMillion.Application.Features.Properties.Cqrs.Commands.UpdateProperty;
using TestMillion.Application.Features.Properties.Cqrs.Queries.GetProperties;
using TestMillion.Application.Features.Properties.DTOs.Response;
using TestMillion.Application.Properties.DTOs.Request;
using TestMillion.Presentation.Controllers.Base;

namespace TestMillion.Presentation.Controllers;

public class PropertiesController : BaseController
{
  
  [HttpGet()]
  [Produces(typeof(ResultResponse<List<PropertyResponseDto>>))]
  [ActionName(nameof(GetAllProperty))]
  public async Task<IActionResult> GetAllProperty()
  {
    var query = new GetPropertyAllQuery();
    var response = await this.Mediator.Send(query);
    return this.FromResult(response);
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
    var query = new GetByIdOwnerQuery(id);
    var response = await this.Mediator.Send(query);
    return this.FromResult(response);
  }

  [HttpPost]
  [Produces(typeof(ResultResponse<PropertyResponseDto>))]
  [ActionName(nameof(CreateOwner))]
  public async Task<IActionResult> CreateOwner([FromBody] UpdatePropertyRequestDto request)
  {
    var command = this.Mapper.Map<CreatePropertyCommand>(request);
    var response = await this.Mediator.Send(command);
    return this.FromResult(response);
  }
}