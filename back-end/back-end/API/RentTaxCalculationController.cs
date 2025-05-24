using Microsoft.AspNetCore.Mvc;
using back_end.Infraestructure;
using back_end.Application;
using System.Security.Claims;

namespace back_end.API
{
  [ApiController]
  [Route("api/[controller]")]
  public class RentTaxCalculationController : ControllerBase
  {
    private readonly RentTax rentTaxCalculation;

    public RentTaxCalculationController()
    {
      var dataBaseConnection = new DataBaseConnectionRepository();
      var rentTaxRepository =
        new RentTaxRepository(dataBaseConnection);

      var monthlyTaxStrategy = new MonthlyEmployeeRentTax();

      rentTaxCalculation = new RentTax(rentTaxRepository
        , monthlyTaxStrategy);
    }

    [HttpGet]
    public ActionResult GetRentTax()
    {
      try
      {
        // string employeeID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // TEST
        string employeeID = "09B49DDE-EBD5-4D00-8871-059FDD92352A";
        if (string.IsNullOrEmpty(employeeID))
          return Unauthorized(new { mensaje = "Usuario no autenticado." });

        var taxAmount = rentTaxCalculation.CalculateRentTax(employeeID);

        return Ok(new
        {
          calculatedTax = taxAmount,
          employee = employeeID
        });
      }
      catch (Exception ex)
      {
        return BadRequest(new { mensaje = ex.Message });
      }
    }
  } 
}
