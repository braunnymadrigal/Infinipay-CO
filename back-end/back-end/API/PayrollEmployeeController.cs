using back_end.Application;
using back_end.Infraestructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollEmployeeController : GeneralController
    {
        private readonly IPayrollEmployeeRepository payrollEmployeeRepository;

        public PayrollEmployeeController()
        {
            payrollEmployeeRepository = new PayrollEmployeeRepository(new ConnectionRepository()
                , new UtilityRepository());
        }

        [Authorize(Roles = "empleador")]
        [HttpGet]
        public IActionResult GetPayrollEmployees(DateOnly startDate, DateOnly endDate)
        {
            IActionResult iActionResult = BadRequest("Unknown error.");
            try
            {
                var employerId = GetUser().PersonId;
                var payrollEmployees = payrollEmployeeRepository.getPayrollEmployees(employerId, startDate, endDate);
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
