using back_end.API;
using back_end.Infraestructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EmployerReportController : GeneralController
{
  private readonly IEmployerReportRepository repository;

  public EmployerReportController()
  {
    repository = new EmployerReportRepository(
        new ConnectionRepository()
    );
  }

  [Authorize(Roles = "empleador")]
  [HttpGet]
  public IActionResult GetEmployerReport()
  {
    try
    {
      var employerId = GetUser().PersonId;
      var report = repository.GetEmployerPayrollReport(employerId);
      return Ok(report);
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { message = ex.Message });
    }
  }
}