using System.Text;
using System.Text.Json;
using back_end.Application;
using back_end.Domain;
using back_end.Infraestructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeductionController : GeneralController
    {
        private readonly IDeduction deduction;

        public DeductionController()
        {
            deduction = new Deduction();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ComputeDeductions(List<PayrollEmployeeModel> 
            payrollEmployees, string paymentType)
        {
            IActionResult iActionResult = BadRequest("Unknown error.");
            try
            {
                payrollEmployees = await deduction.calculateDeductions(payrollEmployees, paymentType);
                iActionResult = Ok(payrollEmployees);
            }
            catch (Exception e)
            {
                iActionResult = NotFound(e.Message);
            }
            return iActionResult;
        }
    }
}
