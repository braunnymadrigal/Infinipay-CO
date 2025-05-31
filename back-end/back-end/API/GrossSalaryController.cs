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
            grossSalary = new GrossSalary(new GrossSalaryRepository(), new ContextGrossSalaryComputation());
        }

        [Authorize(Roles = "empleador")]
        [HttpPost]
        public IActionResult ComputeGrossSalary(DateOnly startDate, DateOnly endDate)
        {
            IActionResult iActionResult = BadRequest("Unknown error.");
            try
            {
                var grossSalaries = CallGrossSalaryMethods(startDate, endDate);
                iActionResult = Ok(grossSalaries);
            }
            catch (Exception e)
            {
                iActionResult = NotFound(e.Message);
            }
            return iActionResult;
        }

        private List<GrossSalaryModel> CallGrossSalaryMethods(DateOnly startDate, DateOnly endDate)
        {
            grossSalary.SetIdEmployer(GetUser().PersonId);
            grossSalary.SetDateRange(startDate, endDate);
            grossSalary.CheckDateRangeCorrectness();
            grossSalary.SetNumberOfWorkedDays();
            return grossSalary.ComputeAllGrossSalaries();
        }
    }
}
