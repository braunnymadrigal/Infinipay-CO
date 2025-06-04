using back_end.Models;

namespace back_end.Application
{
  public interface ICompanyBenefitCommand
  {
    public void CreateBenefit(CompanyBenefitDTO benefit
      , string loggedUserNickname);
    public void UpdateBenefit(Guid id, CompanyBenefitDTO benefit
      , string loggedUserNickname);
  }
}
