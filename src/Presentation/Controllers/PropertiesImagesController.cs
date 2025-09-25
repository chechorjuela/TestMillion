using Microsoft.AspNetCore.Mvc;
using TestMillion.Application.Common.Models;
using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyImage.Cqrs.Commands.CreatePropertyImage;
using TestMillion.Application.Features.PropertyImage.Commands.DeletePropertyImage;
using TestMillion.Application.Features.PropertyImage.Commands.UpdatePropertyImage;
using TestMillion.Application.Features.PropertyImage.Cqrs.Queries.GetAllPropertyImage;
using TestMillion.Application.Features.PropertyImage.Cqrs.Queries.GetByIdPropertyImage;
using TestMillion.Application.Features.PropertyImage.DTOs.Request;
using TestMillion.Application.Features.PropertyImage.DTOs.Response;
using TestMillion.Presentation.Controllers.Base;

namespace TestMillion.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesImagesController : BaseController
{
  [HttpGet]
  [Produces(typeof(PagedResponse<List<PropertyImageResponseDto>>))]
  [ActionName(nameof(GetAllPropertyImage))]
  public async Task<IActionResult> GetAllPropertyImage([FromQuery] PaginationRequestDto pagination, [FromQuery] FilterRequestDto filter = null)
  {
    var query = new GetAllPropertyImageQuery { Pagination = pagination, Filter = filter ?? new FilterRequestDto() };
    var response = await this.Mediator.Send(query);
    return this.FromPagedResult(response);
  }
  
  [HttpGet("{id}")]
  [Produces(typeof(ResultResponse<PropertyImageResponseDto>))]
  [ActionName(nameof(GetPropertyImageById))]
  public async Task<IActionResult> GetPropertyImageById([FromRoute] string id)
  {
    var query = new GetByPropertyImageQuery(id);
    var response = await this.Mediator.Send(query);
    return this.FromResult(response);
  }
  
  [HttpPost]
  [Produces(typeof(ResultResponse<PropertyImageResponseDto>))]
  [ActionName(nameof(CreatePropertyImage))]
  public async Task<IActionResult> CreatePropertyImage(CreatePropertyImageRequestDto request)
  {
    var command = this.Mapper.Map<CreatePropertyImageCommand>(request);
    var response = await this.Mediator.Send(command);
    return this.FromResult(response);
  }
  
  [HttpPut("{id}")]
  [Produces(typeof(ResultResponse<PropertyImageResponseDto>))]
  [ActionName(nameof(UpdatePropertyImage))]
  public async Task<IActionResult> UpdatePropertyImage([FromRoute] string id, [FromBody] UpdatePropertyImageRequestDto request)
  {
    request.Id = id;
    var command = this.Mapper.Map<UpdatePropertyImageCommand>(request);
    var response = await this.Mediator.Send(command);
    return this.FromResult(response);
  }

  [HttpDelete("{id}")]
  //[Produces(typeof(ResultResponse<bool>))]
  [ActionName(nameof(DeletePropertyImage))]
  public async Task<IActionResult> DeletePropertyImage([FromRoute] string id)
  {
    var command = new DeletePropertyImageCommand { Id = id };
    var response = await this.Mediator.Send(command);
    return this.FromResult(response);
  }
}