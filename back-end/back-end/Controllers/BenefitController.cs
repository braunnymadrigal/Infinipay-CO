using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using back_end.Models;
using back_end.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BenefitController : ControllerBase
    {
        private readonly BenefitRepository _benefitRepository;

        public BenefitController()
        {
            _benefitRepository = new BenefitRepository();
        }
        [Authorize(Roles = "empleador")]
        [HttpGet]
        public ActionResult<List<BenefitModel>> GetAllBenefits()
        {
            var nickname = "";
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                var Nickname = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
                if (Nickname != null)
                {
                    nickname = Nickname;
                }
            }
            var benefits = _benefitRepository.GetAllBenefits(nickname);
            return Ok(benefits);
        }

        [Authorize(Roles = "empleador,administrador")]
        [HttpPost]
        public async Task<ActionResult<bool>> CreateBenefit(BenefitModel benefit)
        {
            try
            {
                if (benefit == null)
                    return BadRequest(new { message = "Datos inválidos." });

                string userId = "";
                var identity = HttpContext.User.Identity as ClaimsIdentity;

                if (identity != null)
                {
                    var claims = identity.Claims;
                    var sid = claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
                    if (!string.IsNullOrEmpty(sid))
                        userId = sid;
                }

                benefit.UserCreator = userId;

                var result = _benefitRepository.CreateBenefit(benefit);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("BENEFICIO_DUPLICADO"))
                {
                    return Conflict(new
                    {
                        message = "Error: ya existe un beneficio registrado con ese nombre."
                    });
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Error creando el beneficio.",
                    details = ex.Message
                });
            }
        }
    }
}
