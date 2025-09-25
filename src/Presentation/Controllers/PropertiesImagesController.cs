using Microsoft.AspNetCore.Mvc;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Owners.Cqrs.Commands.CreateOwner;
using TestMillion.Application.Features.Owners.Cqrs.Commands.DeleteOwner;
using TestMillion.Application.Features.Owners.Cqrs.Commands.UpdateOwner;
using TestMillion.Application.Features.Owners.DTOs.Request;
using TestMillion.Application.Features.Owners.DTOs.Response;
using TestMillion.Application.Features.Owners.Cqrs.Queries.GetAllOwner;
using TestMillion.Presentation.Controllers.Base;

namespace TestMillion.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesImagesController : BaseController
{
  [HttpGet]
  //[Produces(typeof(ResultResponse<List>))]
  [ActionName(nameof(GetAllPropertyImage))]
  public async Task<IActionResult> GetAllPropertyImage()
  {
    var query = new GetAllOwnerQuery();
    var response = await this.Mediator.Send(query);
    return this.FromResult(response);
  }
  
  [HttpPost]
  //[Produces(typeof(ResultResponse<OwnerResponseDto>))]
  [ActionName(nameof(CreateOwner))]
  public async Task<IActionResult> CreateOwner(CreateOwnerRequestDto request)
  {
    var command = this.Mapper.Map<CreateOwnerCommand>(request);
    var response = await this.Mediator.Send(command);
    return this.FromResult(response);
  }
  
  [HttpPut("{id}")]
  //[Produces(typeof(ResultResponse<OwnerResponseDto>))]
  [ActionName(nameof(UpdateOwner))]
  public async Task<IActionResult> UpdateOwner([FromRoute] string id, [FromBody] UpdateOwnerRequestDto request)
  {
    request.Id = id;
    var command = this.Mapper.Map<UpdateOwnerCommand>(request);
    var response = await this.Mediator.Send(command);
    return this.FromResult(response);
  }

  [HttpDelete("{id}")]
  //[Produces(typeof(ResultResponse<bool>))]
  [ActionName(nameof(DeleteOwner))]
  public async Task<IActionResult> DeleteOwner([FromRoute] string id)
  {
    var command = new DeleteOwnerCommand { Id = id };
    var response = await this.Mediator.Send(command);
    return this.FromResult(response);
  }
}