using back_end.Application;
using back_end.Domain;
using back_end.Infraestructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end.API
{
  [Route("api/[controller]")]
  [ApiController]
  public class RentTaxController : GeneralController
  {
    private readonly IRentTax rentTax;
    public RentTaxController()
    {
      rentTax = new RentTax();
    }

    [Authorize(Roles = "empleador")]
    [HttpPost]
    public IActionResult ComputeRentTaxes(List<GrossSalaryModel> grossSalaries)
    {
      IActionResult iActionResult = BadRequest("Unknown error.");
      try
      {
        var rentTaxes = rentTax.calculateRentTaxes(grossSalaries);
        iActionResult = Ok(rentTaxes);
      }
      catch (Exception e)
      {
        iActionResult = NotFound(e.Message);
      }
      return iActionResult;
    }
  }
}
