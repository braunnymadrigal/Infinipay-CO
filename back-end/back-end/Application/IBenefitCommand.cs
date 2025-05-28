using back_end.Models;

namespace back_end.Application
{
  public interface IBenefitCommand<T> where T : IBenefitWrapper
  {
    public void CreateBenefit(T benefitWrapper, string loggedUserNickname);
  }
}
