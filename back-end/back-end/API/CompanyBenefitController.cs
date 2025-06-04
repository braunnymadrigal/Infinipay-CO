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
        private readonly IBenefitQuery<CompanyBenefitDTO> companybenefitQuery;
        private readonly ICompanyBenefitCommand companybenefitCommand;

        public CompanyBenefitController(
            IBenefitQuery<CompanyBenefitDTO> companybenefitQuery, 
            ICompanyBenefitCommand companybenefitCommand)
        {
            this.companybenefitQuery = companybenefitQuery;
            this.companybenefitCommand = companybenefitCommand;
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

        [Authorize(Roles = "empleador, administrador")]
        [HttpGet]
        public async Task<ActionResult<List<CompanyBenefitDTO>>> getAll()
        {
            try
            {
                var loggedUserNickname = getLoggedUserNickname();

                if (string.IsNullOrEmpty(loggedUserNickname))
                {
                return NotFound(
                    "No se pudo obtener el nombre de usuario");
                }

                var benefits = companybenefitQuery.getBenefits(loggedUserNickname);

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

        [Authorize(Roles = "empleador, administrador")]
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyBenefitDTO>> getById(Guid id)
        {
            try
            {     
                var benefit = companybenefitQuery.getBenefitById(id);

                if (benefit == null)
                {
                    return NotFound("Beneficio no encontrado");
                }

                return Ok(benefit);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Error obteniendo beneficio",
                    details = ex.Message
                });
            }
        }

        [Authorize(Roles = "empleador,administrador")]
        [HttpPost]
        public async Task<ActionResult<bool>> CreateBenefit([FromBody] CompanyBenefitDTO benefit)
        {
            try
            {
                if (benefit == null)
                    return BadRequest("Datos inválidos.");

                var loggedUserNickname = getLoggedUserClaim(ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(loggedUserNickname))
                {
                    return NotFound("No se pudo obtener el identificador" +
                        " del usuario.");
                }
                companybenefitCommand.CreateBenefit(benefit, loggedUserNickname);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Error creando el beneficio.",
                    details = ex.Message
                });
            }
        }

        [Authorize(Roles = "empleador,administrador")]
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> UpdateBenefit(Guid id, [FromBody] CompanyBenefitDTO benefit)
        {
            try
            {
                if (benefit == null || id == Guid.Empty)
                    return BadRequest("Datos inválidos.");

                var loggedUserNickname = getLoggedUserClaim(ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(loggedUserNickname))
                {
                    return NotFound("Usuario no autenticado");
                }

                companybenefitCommand.UpdateBenefit(id, benefit, loggedUserNickname);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Error actualizando el beneficio.",
                    details = ex.Message
                });
            }
        }
    }
}