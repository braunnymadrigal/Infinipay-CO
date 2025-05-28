using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using back_end.Repositories;
using back_end.Models;

namespace back_end.Application
{
  public class CompanyBenefitCommand : IBenefitCommand<CompanyBenefitDTO>
  {
    private readonly CompanyBenefitRepository _companyBenefitRepository;

    public CompanyBenefitCommand()
    {
      _companyBenefitRepository = new CompanyBenefitRepository();
    }

    public void CreateBenefit(CompanyBenefitDTO benefitWrapper, string loggedUserId)
    {
      _companyBenefitRepository.CreateBenefit(benefitWrapper, loggedUserId);
    }

  }
}