using back_end.Models;

namespace back_end.Repositories
{
  public interface IBenefitRepository<T> where T : IBenefitWrapper
  {
    public List<T> getBenefits(string loggedUserId);
  }
}
