using back_end.Infraestructure;
using back_end.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace back_end.Controllers
{
  [Route("api/[controller]")]
  [ApiController]

  public class AssignedBenefitListController : ControllerBase
  {
    private readonly AssignedBenefitListRepository
      _assignedBenefitListRepository;

    public AssignedBenefitListController()
    {
      _assignedBenefitListRepository = new AssignedBenefitListRepository();

      if (_assignedBenefitListRepository == null)
      {
        throw new NullReferenceException(
          "Error creando AssignedBenefitListRepositor");
      }

    }

    [Authorize(Roles = "supervisor, administrador, sinRol")]
    [HttpGet]
    public ActionResult<List<AssignedBenefitListModel>> Get()
    {
      string logguedId = "";
      var identity = HttpContext.User.Identity as ClaimsIdentity;
      if (identity != null)
      {
        var userClaims = identity.Claims;
        var sid =
          userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value;
        if (!string.IsNullOrEmpty(sid))
        {
          logguedId = sid;
        }
      }

      if (string.IsNullOrWhiteSpace(logguedId))
      {
        return BadRequest(
          "No se pudo obtener el nombre de usuario desde el token/cookie");
      }
      try
      {
        var benefits =
          _assignedBenefitListRepository.GetBenefits(logguedId);

        if (benefits == null)
        {
          return NotFound("Beneficios son nulos");
        }

        return Ok(benefits);
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError,
            new
            {
              message =
            "Error obteniendo beneficios",
              details = ex.Message
            });
      }
    }

    [Authorize(Roles = "supervisor, administrador, sinRol")]
    [HttpPost]
    public async Task<ActionResult<bool>>
        AssignBenefit([FromBody] AssignBenefitRequest request)
    {
      try
      {
        if (request == null)
        {
          return BadRequest();
        }
        string logguedId = "";
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity != null)
        {
          var userClaims = identity.Claims;
          var sid =
            userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value;
          if (sid != null)
          {
            logguedId = sid;
          }
        }
        AssignedBenefitListRepository assignRepository
          = new AssignedBenefitListRepository();
        var assignmentResult = assignRepository.AssignBenefit(request
         , logguedId);
        return new JsonResult(assignmentResult);

      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError
          , new
          {
            message = "Error al asignar beneficio"
            ,
            details = ex.Message
          });
      }
    }
  }

}