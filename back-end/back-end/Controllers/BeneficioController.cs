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
        [HttpGet]
        public List<BenefitModel> GetAllBenefits()
        {
            var benefits = _benefitRepository.GetAllBenefits();
            return benefits;
        }
        [HttpGet ("{id}")]
        public BenefitModel GetBenefitById(int id)
        {
            var benefit = _benefitRepository.GetBenefitById(id);
            return benefit;
        }
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
