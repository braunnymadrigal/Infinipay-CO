using back_end.Infraestructure;
using back_end.Domain;
using back_end.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace back_end.API
{
  [Route("api/[controller]")]
  [ApiController]
  public class EmployeeController : ControllerBase
  {
    private readonly EmployeeRepository _employeeRepository;
    private readonly IEmployeeCommand _employeeCommand;

    public EmployeeController()
    {
      _employeeRepository = new EmployeeRepository();
      _employeeCommand = new EmployeeCommand();
    }

    [Authorize(Roles = "empleador,administrador")]
    [HttpPost]
    public async Task<ActionResult<bool>> createNewEmployee(EmployeeModel
      employee)
    {
      try
      {
        if (employee == null)
        {
          return BadRequest();
        }
        string logguedId = "";
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity != null)
        {
          var userClaims = identity.Claims;
          var sid = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value;
          if (sid != null)
          {
            logguedId = sid;
          }
        }
        EmployeeRepository employeeRepository = new EmployeeRepository();
        var result = employeeRepository.createNewEmployee(employee, logguedId);
        return new JsonResult(result);
      }
      catch (Exception ex)
      {
        var errorResponse = new { message = "", details = ex.Message };

        if (ex.Message.Contains("CEDULA_DUPLICADA"))
        {
          return Conflict(new
          {
            message = "Error: ya existe un empleado registrado con" +
            " esa cédula."
          });
        }
        else if (ex.Message.Contains("TELEFONO_DUPLICADO"))
        {
          return Conflict(new
          {
            message = "Error: ya existe un empleado registrado con ese" +
            " número de teléfono."
          });
        }
        else if (ex.Message.Contains("EMAIL_DUPLICADO"))
        {
          return Conflict(new
          {
            message = "Error: ya existe un empleado  registrado con ese" +
            " correo electrónico."
          });
        }
        else if (ex.Message.Contains("USERNAME_DUPLICADO"))
        {
          return Conflict(new
          {
            message = "Error: ya existe un empleado registrado con ese" +
            " nombre de usuario."
          });
        }
        return StatusCode(StatusCodes.Status500InternalServerError
          , new { message = "Error creando empleado", details = ex.Message });
      }
    }

    [Authorize(Roles = "empleador,administrador")]
    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> UpdateEmployee(Guid id, EmployeeModel employee)
    {
      try
      {
        if (employee == null)
        {
          return BadRequest();
        }
        _employeeCommand.UpdateEmployeeData(employee, id);
        return Ok(true);
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError
          , new { message = "Error actualizando empleado", details = ex.Message });
      }
    }
  }
}
