using back_end.Models;

namespace back_end.Application
{
  public interface ICompanyBenefitQuery
  {
    public List<CompanyBenefitDTO> getBenefits(string loggedUserNickname);
    public CompanyBenefitDTO getBenefitById(Guid id);
    public List<KeyValuePair<string, int>> getBenefitsPerEmployees(string loggedUserNickname);
  }
}
