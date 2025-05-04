using back_end.Repositories;
using back_end.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class EmployeeController : ControllerBase
  {
    private readonly EmployeeRepository _employeeRepository;

    public EmployeeController()
    {
      _employeeRepository = new EmployeeRepository();
    }
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
        EmployeeRepository employeeRepository = new EmployeeRepository();
        var result = employeeRepository.createNewEmployee(employee);
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
  }
}
