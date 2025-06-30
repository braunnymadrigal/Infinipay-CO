using back_end.API;
using back_end.Infraestructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EmployeeReportController : GeneralController
{
  private readonly IEmployeeReportRepository repository;

  public EmployeeReportController()
  {
    repository = new EmployeeReportRepository(
        new ConnectionRepository(),
        new UtilityRepository()
    );
  }

  [Authorize(Roles = "supervisor, administrador, sinRol")]
  [HttpGet]
  public IActionResult GetEmployeeReport()
  {
    try
    {
      var employeeId = GetUser().PersonId;
      var report = repository.GetEmployeePayrollReport(employeeId);
      return Ok(report);
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { message = ex.Message });
    }
  }
}