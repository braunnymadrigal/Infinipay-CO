using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using back_end.Repositories;
using back_end.Models;

namespace back_end.Application
{
  public class EmployeeBenefitQuery : IBenefitQuery<EmployeeBenefitDTO>
  {
    private readonly EmployeeBenefitRepository _employeeBenefitRepository;

    public EmployeeBenefitQuery()
    {
      _employeeBenefitRepository = new EmployeeBenefitRepository();
    }

    public List<EmployeeBenefitDTO> getBenefits(string loggedUserNickname)
    {
      return _employeeBenefitRepository.getBenefits(loggedUserNickname);
    }

    public bool assignBenefit(AssignBenefitRequest request
      , string loggedUserNickname)
    {
      return _employeeBenefitRepository.assignBenefit(request
        , loggedUserNickname);
    }
  }
}
