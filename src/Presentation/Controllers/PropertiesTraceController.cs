using MediatR;
using Microsoft.AspNetCore.Mvc;
using TestMillion.Presentation.Controllers.Base;

namespace TestMillion.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesTraceController : BaseController
{


  // GET
  [HttpGet]
  public IActionResult Index()
  {
    return Ok("PropertiesTraceController is working");
  }
}