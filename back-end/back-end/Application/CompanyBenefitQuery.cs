using back_end.Repositories;
using back_end.Models;

namespace back_end.Application
{
  public class CompanyBenefitQuery : IBenefitQuery<CompanyBenefitDTO>
  {
    private readonly CompanyBenefitRepository companyBenefitRepository;

    public CompanyBenefitQuery(CompanyBenefitRepository companyBenefitRepository)
    {
      this.companyBenefitRepository = companyBenefitRepository;
    }

    public List<CompanyBenefitDTO> getBenefits(string loggedUserNickname)
    {
      List<CompanyBenefitDTO> benefits;
      if (string.IsNullOrWhiteSpace(loggedUserNickname))
      {
        benefits = new List<CompanyBenefitDTO>();
      } else
      {
        benefits = companyBenefitRepository.getBenefits(loggedUserNickname);

        if (benefits == null)
        {
          benefits = new List<CompanyBenefitDTO>();
        }
      }
      return benefits;
    }

    public CompanyBenefitDTO getBenefitById(Guid id)
    {
      try {
        var benefit = companyBenefitRepository.getBenefitById(id);
        return benefit;
      } catch (Exception ex) {
        throw new Exception("Error al obtener el beneficio por ID", ex);
      }
    }
  }
}
