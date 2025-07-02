using back_end.Repositories;
using back_end.Models;

namespace back_end.Application
{
  public class CompanyBenefitQuery : ICompanyBenefitQuery
  {
    private readonly ICompanyBenefitRepository companyBenefitRepository;

    public CompanyBenefitQuery(ICompanyBenefitRepository companyBenefitRepository)
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

    public List<KeyValuePair<string, int>> getBenefitsPerEmployees(string loggedUserNickname)
    {
      if (string.IsNullOrWhiteSpace(loggedUserNickname))
      {
        throw new ArgumentException("El nombre de usuario no puede estar vacío", nameof(loggedUserNickname));
      }
      try
      {
        var benefits = companyBenefitRepository.GetBenefitsPerEmployees(loggedUserNickname);
        if (benefits == null || benefits.Count == 0)
        {
          throw new Exception("No se encontraron beneficios por empleados");
        }
        return benefits;
      }
      catch (Exception ex)
      {
        throw new Exception("Error al obtener los beneficios por empleados", ex);
      }
    }
  }
}
