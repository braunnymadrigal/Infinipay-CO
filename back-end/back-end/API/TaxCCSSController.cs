using back_end.Application;
using back_end.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxCCSSController : GeneralController
    {
        private readonly ITaxCCSS taxCCSS;
        public TaxCCSSController()
        {
            taxCCSS = new TaxCCSS();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ComputeTaxesCCSS(List<PayrollEmployeeModel> payrollEmployees)
        {
            IActionResult iActionResult = BadRequest("Unknown error.");
            try
            {
                var taxesCCSS = taxCCSS.ComputeTaxesCCSS(payrollEmployees);
                iActionResult = Ok(taxesCCSS);
            }
            catch (Exception e)
            {
                iActionResult = NotFound(e.Message);
            }
            return iActionResult;
        }
    }
}
