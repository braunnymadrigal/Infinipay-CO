using back_end.Application;
using back_end.Domain;
using back_end.Infraestructure;
using Microsoft.AspNetCore.Mvc;

namespace back_end.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollEmployeeController : GeneralController
    {
        private readonly IPayrollEmployee payrollEmployee;

        public PayrollEmployeeController()
        {
            payrollEmployee = new PayrollEmployee(
                new PayrollEmployeeRepository(new ConnectionRepository()
                , new UtilityRepository()));
        }

        [HttpGet]
        public IActionResult GetPayrollEmployees(PayrollEmployerModel payrollEmployer)
        {
            IActionResult iActionResult = BadRequest("Unknown error.");
            try
            {
                var payrollEmployees = payrollEmployee.getPayrollEmployees(payrollEmployer);
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
