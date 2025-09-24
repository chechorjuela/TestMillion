using MediatR;
using Microsoft.AspNetCore.Mvc;
using TestMillion.Presentation.Controllers.Base;

namespace TestMillion.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesImagesController : BaseController
{

  // GET
  [HttpGet]
  public IActionResult Index()
  {
    return Ok("PropertiesImagesController is working");
  }
}