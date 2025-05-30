using back_end.Application;
using back_end.Domain;

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
            grossSalary = new GrossSalary();
        }

        [Authorize(Roles = "empleador")]
        [HttpPost]
        public IActionResult ComputeGrossSalary(DateOnly startDate, DateOnly endDate)
        {
            IActionResult iActionResult = BadRequest("Unknown error.");
            try
            {
                CallGrossSalaryMethods(startDate, endDate);
                iActionResult = Ok();
            }
            catch (Exception e)
            {
                iActionResult = NotFound(e.Message);
            }
            return iActionResult;
        }

        private void CallGrossSalaryMethods(DateOnly startDate, DateOnly endDate)
        {
            grossSalary.SetIdEmployer(GetUser().PersonId);
            grossSalary.SetDateRange(startDate, endDate);
            grossSalary.CheckDateRangeCorrectness();
            grossSalary.SetNumberOfWorkedDays();
        }
    }
}
