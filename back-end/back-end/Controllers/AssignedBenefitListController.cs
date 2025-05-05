using back_end.Repositories;
using back_end.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace back_end.Controllers
{
  [Route("api/[controller]")]
  [ApiController]

  public class AssignedBenefitListController : ControllerBase
  {
    private readonly AssignedBenefitListRepository
      _assignedBenefitListRepository;

    public AssignedBenefitListController()
    {
      _assignedBenefitListRepository = new AssignedBenefitListRepository();
    }

    [HttpGet]
    public List<AssignedBenefitListModel> Get([FromQuery] string userNickname)
    {
      var benefits = _assignedBenefitListRepository.GetBenefits(userNickname);
      return benefits;
    }
  }
}