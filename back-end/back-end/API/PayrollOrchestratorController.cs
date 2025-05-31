using back_end.Application;
using back_end.Domain;
using back_end.Infraestructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace back_end.API
{
  [ApiController]
  [Route("api/[controller]")]
  public class PayrollOrchestratorController : GeneralController
  {
    private readonly IGrossSalary _grossSalary;
    private readonly IRentTax _rentTax;
    private readonly ITaxCCSS _taxCCSS;

    public PayrollOrchestratorController()
    {
      _grossSalary = new GrossSalary(
          new GrossSalaryRepository(
              new ConnectionRepository(),
              new UtilityRepository()
          ),
          new ContextGrossSalaryComputation()
      );
      _rentTax = new RentTax();
      _taxCCSS = new TaxCCSS();
    }

    [Authorize(Roles = "empleador")]
    [HttpPost]
    public IActionResult generatePayroll(PayrollRequestModel payload)
    {
      try
      {
        var startDate = DateOnly.Parse(payload.StartDate);
        var endDate = DateOnly.Parse(payload.EndDate);
        var grossSalaries = obtainGrossSalary(startDate, endDate);

        var rentTaxes = _rentTax.calculateRentTaxes(grossSalaries);
        var ccssTaxes = new TaxCCSS().ComputeTaxesCCSS(grossSalaries);

        var employeePayroll = grossSalaries.Select((s, i) =>
        {
          var rentTaxModel = rentTaxes[i];
          var ccssTaxModel = ccssTaxes[i];

          return new
          {
            s.EmployeeId,
            s.EmployeeName,
            s.HiringDate,
            s.HiringType,
            s.ComputedGrossSalary,
            RentTax = rentTaxModel.rentTax,
            CcssTax = ccssTaxModel.EmployeeAmount,
          };
        });

        return Ok(employeePayroll);
      }
      catch (Exception ex)
      {
        return StatusCode(500, ex.Message);
      }
    }

    private List<GrossSalaryModel> obtainGrossSalary(DateOnly startDate
      , DateOnly endDate)
    {
      _grossSalary.SetIdEmployer(GetUser().PersonId);
      _grossSalary.SetDateRange(startDate.ToString("yyyy-MM-dd")
        , endDate.ToString("yyyy-MM-dd"));
      _grossSalary.SetNumberOfWorkedDays();
      return _grossSalary.ComputeAllGrossSalaries();
    }
  }
}