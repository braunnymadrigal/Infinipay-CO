using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using back_end.Models;
using back_end.Repositories;
using System.Security.Cryptography.X509Certificates;

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
        [HttpGet("user/{nicknameOrEmail}")]
        public ActionResult<List<BenefitModel>> GetAllBenefits(string nicknameOrEmail)
        {
            var user = new LoginUserModel { NicknameOrEmail = nicknameOrEmail };
            var benefits = _benefitRepository.GetAllBenefits(user);
            return Ok(benefits);
        }

        //[HttpGet("empresa/{idEmpresa}/beneficio/{id}")]
        //public BenefitModel GetBenefitById(Guid idEmpresa, Guid id)
        //{
        //    var benefit = _benefitRepository.GetBenefitById(idEmpresa, id);
        //    return benefit;
        //}

        [HttpPost]
        public async Task<ActionResult<bool>> CreateBenefit(BenefitModel benefit)
        {
            try
            {
                if (benefit == null)
                {
                    return BadRequest();
                }

                BenefitRepository benefitRepository = new BenefitRepository();
                var result = benefitRepository.CreateBenefit(benefit);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creando un beneficio");
            }
        }

    }
}
