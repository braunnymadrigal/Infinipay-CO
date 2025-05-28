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
    public class CompanyBenefitController : ControllerBase
    {
        private readonly CompanyBenefitQuery _companybenefitQuery;
        private readonly CompanyBenefitCommand _companybenefitCommand;

        public CompanyBenefitController()
        {
            _companybenefitQuery = new CompanyBenefitQuery();
            _companybenefitCommand = new CompanyBenefitCommand();
        }

        private string GetLoggedUserClaim(string claimType)
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

        [Authorize(Roles = "empleador, administrador")]
        [HttpGet]
        public ActionResult<List<CompanyBenefitDTO>> GetAllCompanyBenefits()
        {
            var loggedUserNickname = GetLoggedUserClaim(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedUserNickname))
            {
                return BadRequest("No se pudo obtener el nombre de usuario desde el token/cookie");
            }
            var benefits = _companybenefitQuery.getBenefits(loggedUserNickname);
            if (benefits == null || benefits.Count == 0)
            {
                return NotFound("Beneficios no encontrados");
            }
            return Ok(benefits);
        }

        [Authorize(Roles = "empleador,administrador")]
        [HttpPost]
        public ActionResult<bool> CreateBenefit(CompanyBenefitDTO benefit)
        {
            try
            {
                if (benefit == null)
                    return BadRequest("Datos inválidos.");

                var loggedUserNickname = GetLoggedUserClaim(ClaimTypes.NameIdentifier);
                
                if (loggedUserNickname == null)
                {
                    return BadRequest(
                        "No se pudo obtener el nombre de usuario desde el token/cookie");
                }
                _companybenefitCommand.CreateBenefit(benefit, loggedUserNickname);
                return Ok(new
                {
                    message = "Beneficio creado exitosamente."
                });
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