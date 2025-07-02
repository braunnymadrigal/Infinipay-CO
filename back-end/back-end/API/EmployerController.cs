using back_end.Infraestructure;
using back_end.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace back_end.API
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
          return BadRequest(new { message
            = "Datos del empleador no proporcionados." });
        }

        var result = _employerRepository.createNewEmployer(employer);
        return new JsonResult(result);
      }
      catch (SqlException sqlEx)
      {
        if (sqlEx.Message.Contains("CEDULA_DUPLICADA"))
        {
          return Conflict(new { message
            = "Error: ya existe un empleador registrado con esa cédula." });
        }
        else if (sqlEx.Message.Contains("EMAIL_DUPLICADO"))
        {
          return Conflict(new { message
            = "Error: ya existe un empleador registrado con ese correo electrónico." });
        }
        else if (sqlEx.Message.Contains("TELEFONO_DUPLICADO"))
        {
          return Conflict(new { message
            = "Error: ya existe un empleador registrado con ese número de teléfono." });
        }
        else if (sqlEx.Message.Contains("USERNAME_DUPLICADO"))
        {
          return Conflict(new { message
            = "Error: ya existe un empleador registrado con ese nombre de usuario." });
        }

        return StatusCode(StatusCodes.Status500InternalServerError,
          new { message = "Error en la base de datos", details = sqlEx.Message });
      }

    }
  }
}
