using back_end.Application;
using back_end.Domain;
using back_end.Infraestructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollEmployerController : GeneralController
    {
        private readonly IPayrollEmployer payrollEmployer;

        public PayrollEmployerController()
        {
            payrollEmployer = new PayrollEmployer(
                new PayrollEmployerRepository(new ConnectionRepository()
                , new UtilityRepository()));
        }

        //[AllowAnonymous]
        //[HttpPost]
        //public IActionResult GetPayrollEmployees(PayrollEmployerModel payrollEmployer)
        //{
        //    IActionResult iActionResult = BadRequest("Unknown error.");
        //    try
        //    {
        //        var payrollEmployees = payrollEmployee.getPayrollEmployees(payrollEmployer);
        //        iActionResult = Ok(payrollEmployees);
        //    }
        //    catch (Exception e)
        //    {
        //        iActionResult = NotFound(e.Message);
        //    }
        //    return iActionResult;
        //}

        [Authorize(Roles = "empleador")]
        [HttpGet]
        public IActionResult GetPaymentType()
        {
            IActionResult iActionResult = BadRequest("Unknown error.");
            try
            {
                var id = GetUser().PersonId;
                var paymentType = payrollEmployer.getPaymentType(id);
                iActionResult = Ok(paymentType);
            }
            catch (Exception e)
            {
                iActionResult = NotFound(e.Message);
            }
            return iActionResult;
        }
    }
}
