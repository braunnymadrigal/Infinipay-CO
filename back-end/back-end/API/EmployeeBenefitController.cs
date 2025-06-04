using back_end.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using back_end.Models;
using System.Security.Claims;
using back_end.Repositories;
using System;

namespace back_end.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class EmployeeBenefitController : ControllerBase
  {
    private readonly IBenefitQuery<EmployeeBenefitDTO> employeeBenefitQuery;
    private readonly IEmployeeBenefitAssignment employeeBenefitAssignment;
    public EmployeeBenefitController(
      IBenefitQuery<EmployeeBenefitDTO> employeeBenefitQuery
      , IEmployeeBenefitAssignment employeeBenefitAssignment
    )
    {
      this.employeeBenefitQuery = employeeBenefitQuery;
      this.employeeBenefitAssignment = employeeBenefitAssignment;
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

    private string getLoggedUserNickname()
    {
      var loggedUser = getLoggedUserClaim(ClaimTypes.NameIdentifier);

      if (string.IsNullOrEmpty(loggedUser))
      {
        loggedUser = "";
      }

      return loggedUser;
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
    public ActionResult<List<EmployeeBenefitDTO>> get()
    {
      try
      {
        var loggedUserNickname = getLoggedUserNickname();

        if (string.IsNullOrEmpty(loggedUserNickname))
        {
          return NotFound(
            "No se pudo obtener el nombre de usuario");
        }

        var benefits = employeeBenefitQuery.getBenefits(loggedUserNickname);

        if (benefits == null)
        {
          return NotFound("Beneficios no encontrados");
        }

        return Ok(benefits);
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new
        {
          message = "Error obteniendo beneficios",
          details = ex.Message
        });
      }
    }


    [Authorize(Roles = "supervisor, administrador, sinRol")]
    [HttpPost]
    public ActionResult<bool> assignBenefit([FromBody]
      AssignBenefitReq request)
    {
      try
      {
        if (request == null)
        {
          return BadRequest("La solicitud está vacía.");
        }

        var loggedUserId = getLoggedUserId();

        if (string.IsNullOrWhiteSpace(loggedUserId))
        {
          return NotFound("No se pudo obtener el identificador" +
            " del usuario.");
        }

        var assignmentResult = employeeBenefitAssignment.assignBenefit(request
          , loggedUserId);

        if (!assignmentResult)
        {
          return StatusCode(StatusCodes.Status500InternalServerError
            , "No se pudo asignar el beneficio.");
        }
        
        return Ok(assignmentResult);
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new
        {
          message = "Error al asignar beneficio",
          details = ex.Message
        });
      }
    }
  }
}
