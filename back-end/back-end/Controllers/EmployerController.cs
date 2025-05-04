using back_end.Repositories;
using back_end.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers
{
  public class ErrorResponse
  {
    public string Message { get; set; }
    public string Details { get; set; }
  }

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
      catch (Exception ex)
      {
        var errorResponse = new { message = "", details = ex.Message };

        if (ex.Message.Contains("CEDULA_DUPLICADA"))
        {
          return Conflict(new {
            message = "Error: ya existe un empleador registrado con" +
            "esa cédula." });
        }
        else if (ex.Message.Contains("TELEFONO_DUPLICADO"))
        {
          return Conflict(new {
            message = "Error: ya existe un empleador registrado con ese" +
            "número de teléfono." });
        }
        else if (ex.Message.Contains("EMAIL_DUPLICADO"))
        {
          return Conflict(new {
            message = "Error: ya existe un empleador registrado con ese" +
            "correo electrónico." });
        }
        else if (ex.Message.Contains("USERNAME_DUPLICADO"))
        {
          return Conflict(new {
            message = "Error: ya existe un empleador registrado con ese" +
            "nombre de usuario." });
        }
        return StatusCode(StatusCodes.Status500InternalServerError
          , new { message = "Error creando empleador", details = ex.Message });
      }
    }
  }
}
