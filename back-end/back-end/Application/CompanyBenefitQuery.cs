using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using back_end.Repositories;
using back_end.Models;

namespace back_end.Application
{
  public class CompanyBenefitQuery : IBenefitQuery<CompanyBenefitDTO>
  {
    private readonly CompanyBenefitRepository _companyBenefitRepository;

    public CompanyBenefitQuery()
    {
      _companyBenefitRepository = new CompanyBenefitRepository();
    }

    public List<CompanyBenefitDTO> getBenefits(string loggedUserNickname)
    {
      return _companyBenefitRepository.getBenefits(loggedUserNickname);
    }
  }
}
