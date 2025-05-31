using back_end.Application;
using back_end.Domain;
using back_end.Infraestructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxCCSSController : GeneralController
    {
        //private readonly ITaxCCSS taxCCSS;
        public TaxCCSSController()
        {
            //taxCCSS = new TaxCCSS(); 
        }

        [Authorize(Roles = "empleador")]
        [HttpPost]
        public IActionResult ComputeTaxesCCSS(List<GrossSalaryModel> grossSalaries)
        {
            IActionResult iActionResult = BadRequest("Unknown error.");
            try
            {
                var taxesCCSS = CallTaxCCSSMethods(grossSalaries);
                iActionResult = Ok();
            }
            catch (Exception e)
            {
                iActionResult = NotFound(e.Message);
            }
            return iActionResult;
        }

        private List<TaxCCSSModel> CallTaxCCSSMethods(List<GrossSalaryModel> grossSalaries)
        {
            return new List<TaxCCSSModel>();
        }
    }
}
