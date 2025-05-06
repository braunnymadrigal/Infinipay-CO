using back_end.Repositories;
using back_end.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


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

    [HttpGet]
    public ActionResult<List<AssignedBenefitListModel>> Get([FromQuery] 
      string userNickname)
    {
      if (string.IsNullOrWhiteSpace(userNickname))
      {
        return BadRequest("Nombre de usuario es requerido");
      }

      try
      {
        var benefits = _assignedBenefitListRepository.GetBenefits(userNickname);

        if (benefits == null)
        {
          return NotFound("Beneficios son nulos");
        }

        return Ok(benefits);
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, 
          new {message = "Error obteniendo beneficios", details = ex.Message});
      }
    }


    [HttpPost("AssignBenefit")]
    public async Task<ActionResult<bool>> 
        AssignBenefit([FromBody] AssignBenefitRequest request)
      {
        try
        {
          if (request == null)
          {
            return BadRequest();
          }
          AssignedBenefitListRepository assignRepository 
            = new AssignedBenefitListRepository();
           var assignmentResult = assignRepository.AssignBenefit(request);
           return new JsonResult(assignmentResult);

        } catch (Exception ex)
        {
          return StatusCode(StatusCodes.Status500InternalServerError
            , new { message = "Error al asignar beneficio"
              , details = ex.Message });
        }
      }
  }

}