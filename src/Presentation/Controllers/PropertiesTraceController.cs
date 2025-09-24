using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TestMillion.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesTraceController : ControllerBase
{
  private readonly IMediator _mediator;

  public PropertiesTraceController(IMediator mediator)
  {
    _mediator = mediator;
  }

  // GET
  [HttpGet]
  public IActionResult Index()
  {
    return Ok("PropertiesTraceController is working");
  }
}