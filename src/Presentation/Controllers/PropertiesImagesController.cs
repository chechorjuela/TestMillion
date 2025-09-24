using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TestMillion.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesImagesController : ControllerBase
{
  private readonly IMediator _mediator;

  public PropertiesImagesController(IMediator mediator)
  {
    _mediator = mediator;
  }
  // GET
  [HttpGet]
  public IActionResult Index()
  {
    return Ok("PropertiesImagesController is working");
  }
}