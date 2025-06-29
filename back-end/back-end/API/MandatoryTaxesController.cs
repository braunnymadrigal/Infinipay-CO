using back_end.Application;
using back_end.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MandatoryTaxesController : GeneralController
    {
        private readonly IMandatoryTaxes mandatoryTaxes;

        public MandatoryTaxesController()
        {
            mandatoryTaxes = new MandatoryTaxes();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CalculateMandatoryTaxes(List<PayrollEmployeeModel> payrollEmployees)
        {
            IActionResult iActionResult = BadRequest("Unknown error.");
            try
            {
                var taxes = mandatoryTaxes.calculateMandatoryTaxes(payrollEmployees);
                iActionResult = Ok(taxes);
            }
            catch (Exception e)
            {
                iActionResult = NotFound(e.Message);
            }
            return iActionResult;
        }
    }
}
