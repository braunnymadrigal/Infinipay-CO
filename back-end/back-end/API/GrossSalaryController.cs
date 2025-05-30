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
                var id = GetUser().PersonId;
                grossSalary.CheckDateRangeCorrectness(startDate, endDate);
                iActionResult = Ok();
            }
            catch (Exception e)
            {
                iActionResult = NotFound(e.Message);
            }
            return iActionResult;
        }
    }
}
