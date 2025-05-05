using back_end.Repositories;
using back_end.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace back_end.Controllers
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
    [HttpGet]
    public List<EmployeeListModel> Get()
    {
      var employees = _employeeListRepository.obtainEmployeeInfo();
      return employees;
    }
  }
}