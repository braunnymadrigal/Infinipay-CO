using back_end.API;
using back_end.Application;
using back_end.Domain;
using back_end.Infraestructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

[ApiController]
[Route("api/[controller]")]
public class PayrollOrchestratorController : GeneralController
{
  private readonly PayrollOrchestrator payrollService;
  private readonly PayrollOrchestratorRepository repository;

  public PayrollOrchestratorController()
  {
    repository = new PayrollOrchestratorRepository(
        new ConnectionRepository(),
        new UtilityRepository()
    );

    payrollService = new PayrollOrchestrator(
        new PayrollEmployee(new PayrollEmployeeRepository
        (new ConnectionRepository(), new UtilityRepository())),
        new GrossSalary(new ContextGrossSalaryComputation()),
        new RentTax(),
        new TaxCCSS(),
        new Deduction());
  }
  [Authorize(Roles = "empleador")]
  [HttpPost]
  public async Task<IActionResult> ComputePayroll([FromBody] PayrollRequest
    request)
  {
    try
    {
      var employerId = GetUser().PersonId;

      DateOnly start = DateOnly.Parse(request.StartDate);
      DateOnly end = DateOnly.Parse(request.EndDate);

      var result = await payrollService.ComputePayrollAsync(employerId, start
        , end);

      repository.SavePayrollData(employerId, start, end, result);

      return Ok(result);
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { message = ex.Message });
    }
  }

  [Authorize(Roles = "empleador")]
  [HttpGet]
  public IActionResult GetPayrolls()
  {
    try
    {
      var employerId = GetUser().PersonId;
      var data = repository.getPayrollsByEmployerId(employerId);
      return Ok(data);
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { message = ex.Message });
    }
  }

}
