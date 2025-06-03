using back_end.Models;

namespace back_end.Application
{
  public interface IEmployeeBenefitAssignment
  {
    public bool assignBenefit(AssignBenefitRequest request
      , string loggedUserNickname);
  }
}
