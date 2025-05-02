using back_end.Handlers;
using back_end.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class EmployerController : ControllerBase
  {
    private readonly EmployerRepository _employerRepository;
    
    public EmployerController()
    {
      _employerRepository = new EmployerRepository();
    }
    [HttpPost]
    public async Task<ActionResult<bool>>createNewEmployer(EmployerModel
      employer)
    {
      try
      {
        if (employer == null)
        {
          return BadRequest();
        }
        EmployerRepository employerRepository = new EmployerRepository();
        var result = employerRepository.createNewEmployer(employer);
        return new JsonResult(result);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError
          , "Error creando empleador");
      }
    }
  }
}
