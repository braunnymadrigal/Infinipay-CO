using back_end.Infraestructure;
using back_end.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using back_end.Application;


namespace back_end.API
{
	[Route("api/[controller]")]
	public class EmployeeHoursController : ControllerBase
	{
		private readonly IEmployeeHoursQuery employeeHoursQuery;
    private readonly IEmployeeHoursCommand employeeHoursCommand;
		public EmployeeHoursController(IEmployeeHoursQuery employeeHoursQuery
      , IEmployeeHoursCommand employeeHoursCommand)
		{
			this.employeeHoursQuery = employeeHoursQuery;
      this.employeeHoursCommand = employeeHoursCommand;
		}

    private string getLoggedUserClaim(string claimType)
    {
      var identity = HttpContext.User.Identity as ClaimsIdentity;
      if (identity != null)
      {
        var claimValue = identity.Claims
          .FirstOrDefault(c => c.Type == claimType)?.Value;
        if (!string.IsNullOrEmpty(claimValue))
        {
          return claimValue;
        }
      }
      return "";
    }

    private string getLoggedUserId()
    {
      var loggedUserId = getLoggedUserClaim(ClaimTypes.Sid);

      if (string.IsNullOrEmpty(loggedUserId))
      {
        loggedUserId = "";
      }

      return loggedUserId;
    }

[Authorize(Roles = "supervisor, administrador, sinRol")]
    [HttpGet]
		public ActionResult<EmployeeHoursModel> get()
		{
      try
      {
        var loggedUserId = getLoggedUserId();

        if (string.IsNullOrEmpty(loggedUserId))
        {
          return NotFound(
            "No se pudo obtener el identificador de usuario");
        }

        var employeeHoursModel
          = employeeHoursQuery.getEmployeeHoursContract(loggedUserId);

        if (employeeHoursModel == null)
        {
          return NotFound("Horas de empleado no encontrado");
        }

        return Ok(employeeHoursModel);
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new
        {
          message = "Error obteniendo horas de empleado",
          details = ex.Message
        });
      }
		}

    [Authorize(Roles = "supervisor, administrador, sinRol")]
    [HttpGet("Hours")]
    public ActionResult<List<HoursModel>> get([FromQuery] DateOnly startDate,
      DateOnly endDate)
    {
      try
      {
        var loggedUserId = getLoggedUserId();

        if (string.IsNullOrEmpty(loggedUserId))
        {
          return NotFound(
            "No se pudo obtener el identificador de usuario");
        }

        var employeeHoursModel
          = employeeHoursQuery.getEmployeeHoursList(loggedUserId
          , startDate, endDate);

        if (employeeHoursModel == null)
        {
          return NotFound("Contrato de empleado no encontrado");
        }

        return Ok(employeeHoursModel);
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new
        {
          message = "Error obteniendo detalles de contrato",
          details = ex.Message
        });
      }
    }

    [Authorize(Roles = "supervisor, administrador, sinRol")]
    [HttpPost]
    public ActionResult<bool> post([FromBody] List<HoursModel>
      employeeHoursWorked)
    {
      try
      {
        var loggedUserId = getLoggedUserId();

        if (string.IsNullOrEmpty(loggedUserId))
        {
          return NotFound(
            "No se pudo obtener el identificador de usuario");
        }

        var success = employeeHoursCommand.registerEmployeeHours(loggedUserId
          , employeeHoursWorked);

        if (success)
        {
          return Ok(success);
        } else
        {
          return BadRequest(success);
        }
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new
        {
          message = "Error registrando horas de empleado",
          details = ex.Message
        });
      }
    }
  }
}
