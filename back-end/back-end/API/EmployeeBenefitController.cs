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
    private readonly EmployeeBenefitQuery _employeeBenefitQuery;
    public EmployeeBenefitController()
    {
      _employeeBenefitQuery = new EmployeeBenefitQuery();
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


    [Authorize(Roles = "supervisor, administrador, sinRol")]
    [HttpGet]
    public ActionResult<List<EmployeeBenefitDTO>> Get()
    {
      try
      {
        var loggedUserNickname = getLoggedUserClaim(ClaimTypes.NameIdentifier);

        if (loggedUserNickname == null)
        {
          return BadRequest(
            "No se pudo obtener el nombre de usuario desde el token/cookie");
        }

        var benefits = _employeeBenefitQuery.getBenefits(loggedUserNickname);

        if (benefits == null || benefits.Count == 0)
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
    public ActionResult<bool> AssignBenefit([FromBody]
      AssignBenefitRequest request)
    {
      try
      {
        if (request == null)
        {
          return BadRequest("La solicitud está vacía.");
        }

        var loggedUserId = getLoggedUserClaim(ClaimTypes.Sid);

        if (string.IsNullOrWhiteSpace(loggedUserId))
        {
          return Unauthorized("No se pudo obtener el usuario.");
        }
        var assignmentResult = _employeeBenefitQuery.assignBenefit(request
          , loggedUserId);

        if (!assignmentResult)
        {
          return StatusCode(StatusCodes.Status500InternalServerError
            , "No se pudo asignar el beneficio.");
        }
        
        return Ok(true);
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
