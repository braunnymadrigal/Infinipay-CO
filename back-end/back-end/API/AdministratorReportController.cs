using back_end.Application;
using back_end.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end.API
{
  [ApiController]
  [Route("api/[controller]")]
  public class AdministratorReportController : GeneralController
  {
    private readonly IAdministratorReportQuery administratorReportQuery;

    public AdministratorReportController(IAdministratorReportQuery
      administratorReportQuery)
    {
      this.administratorReportQuery = administratorReportQuery;
    }

    //[Authorize(Roles = "superAdmin")]
    [HttpGet]
    public ActionResult<List<PayrollAdministratorModel>>
      getAllCompaniesPayroll([FromQuery] DateOnly startDate
      , [FromQuery] DateOnly endDate)
    {
      try
      {
        var companyPayroll
          = administratorReportQuery.getAllCompaniesPayroll(startDate, endDate);

        return Ok(companyPayroll);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}
