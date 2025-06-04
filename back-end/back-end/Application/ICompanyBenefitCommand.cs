using back_end.Models;

namespace back_end.Application
{
  public interface ICompanyBenefitCommand
  {
    public void CreateBenefit(IBenefitWrapper benefitWrapper, string loggedUserNickname);
  }
}
