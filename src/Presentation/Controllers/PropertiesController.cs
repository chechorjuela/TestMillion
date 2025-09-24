using Microsoft.AspNetCore.Mvc;
using TestMillion.Application.Common.Models;
using TestMillion.Application.DTOs;
using TestMillion.Application.Features.Owners.Queries.GetAllOwner;
using TestMillion.Application.Properties.Commands.CreateProperty;
using TestMillion.Application.Properties.DTOs.Request;
using TestMillion.Application.Properties.Queries.GetProperties;
using TestMillion.Application.Properties.Queries.GetPropertyDetail;
using TestMillion.Presentation.Controllers.Base;

namespace TestMillion.Presentation.Controllers;

public class PropertiesController : BaseController
{
  [HttpGet]
  public async Task<IActionResult> GetAllOwner()
  {
    var query = new GetAllOwnerQuery();
    var response = await this.Mediator.Send(query);
    return Ok();
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetPropertyDetail(string id)
  {
    var query = new GetPropertyDetailQuery { Id = id };
    var result = await this.Mediator.Send(query);
    return Ok();
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreatePropertyRequestDto request)
  {
    var command = new CreatePropertyCommand(request);
    var result = await this.Mediator.Send(command);
    return Ok();
  }
}