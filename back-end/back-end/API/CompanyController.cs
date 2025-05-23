using back_end.Infraestructure;
using back_end.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace back_end.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CompanyController : ControllerBase
  {
    private readonly CompanyRepository _companyRepository;
    public CompanyController()
    {
      _companyRepository = new CompanyRepository();
    }
    [HttpGet]
    public List<CompanyModel> GetAllCompanies()
    {
        var companies = _companyRepository.GetAllCompanies();
        return companies;
    }
    [HttpPost]
    public async Task<ActionResult<bool>> createNewCompany(CompanyModel
      company)
    {
      try
      {
        if (company == null)
        {
          return BadRequest();
        }
        CompanyRepository companyRepository = new CompanyRepository();
        var result = companyRepository.createNewCompany(company);
        return new JsonResult(result);
      }
      catch (Exception ex)
      {
        var errorResponse = new { message = "", details = ex.Message };
        if (ex.Message.Contains("CEDULA_DUPLICADA"))
        {
          return Conflict(new
          {
            message = "Error: ya existe una empresa registrada con" +
            " esa cédula."
          });
        }
        else if (ex.Message.Contains("TELEFONO_DUPLICADO"))
        {
          return Conflict(new
          {
            message = "Error: ya existe una empresa registrada con ese" +
            " número de teléfono."
          });
        }
        else if (ex.Message.Contains("EMAIL_DUPLICADO"))
        {
          return Conflict(new
          {
            message = "Error: ya existe una empresa registrado con ese" +
            " correo electrónico."
          });
        }
        else if (ex.Message.Contains("USERNAME_DUPLICADO"))
        {
          return Conflict(new
          {
            message = "Error: ya existe una empresa registrado con ese" +
            " nombre de usuario."
          });
        }
        return StatusCode(StatusCodes.Status500InternalServerError
          , new { message = "Error creando empresa", details = ex.Message });
      }
    }
  }
}