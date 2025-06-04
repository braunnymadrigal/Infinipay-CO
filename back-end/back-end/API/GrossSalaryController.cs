using back_end.Application;
using back_end.Domain;
using back_end.Infraestructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrossSalaryController : GeneralController
    {
        private readonly IGrossSalary grossSalary;

        public GrossSalaryController()
        {
            grossSalary = new GrossSalary (new ContextGrossSalaryComputation());
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ComputeGrossSalary(List<PayrollEmployeeModel> payrollEmployees, 
            DateOnly startDate, DateOnly endDate)
        {
            IActionResult iActionResult = BadRequest("Unknown error.");
            try
            {
                var grossSalaries = grossSalary.computeAllGrossSalaries(payrollEmployees, 
                    startDate, endDate);
                iActionResult = Ok(grossSalaries);
            }
            catch (Exception e)
            {
                iActionResult = NotFound(e.Message);
            }
            return iActionResult;
        }
    }
}
