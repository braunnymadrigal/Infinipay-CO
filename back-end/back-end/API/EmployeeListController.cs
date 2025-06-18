using back_end.Infraestructure;
using back_end.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace back_end.API
{
  [Route("api/[controller]")]
  [ApiController]
  public class EmployeeListController : ControllerBase
  {
    private readonly EmployeeListRepository _employeeListRepository;
    public EmployeeListController()
    {
      _employeeListRepository = new EmployeeListRepository();
    }

    [Authorize(Roles = "empleador, supervisor, administrador")]
    [HttpGet]
    public List<EmployeeListModel> Get()
    {
      string logguedId = "";
      string role = "";

      var identity = HttpContext.User.Identity as ClaimsIdentity;
      if (identity != null)
      {
        var userClaims = identity.Claims;
        logguedId = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value;
        role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value;
      }

      var employees = _employeeListRepository.obtainEmployeeInfo(logguedId, role);
      return employees;
    }

  }
}