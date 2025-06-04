using back_end.Models;

namespace back_end.Application
{
  public interface IBenefitQuery<T> where T : IBenefitWrapper
  {
    public List<T> getBenefits(string loggedUserNickname);
    public T getBenefitById(Guid id);
  }
}
