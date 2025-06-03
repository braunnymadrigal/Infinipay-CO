using back_end.Repositories;
using back_end.Models;

namespace back_end.Application
{
  public class EmployeeBenefitAssignment : IEmployeeBenefitAssignment
  {
    private readonly EmployeeBenefitRepository employeeBenefitRepository;

    public EmployeeBenefitAssignment(EmployeeBenefitRepository
      employeeBenefitRepository)
    {
      this.employeeBenefitRepository = employeeBenefitRepository;
    }

    public bool assignBenefit(AssignBenefitRequest request
      , string loggedUserNickname)
    {
      if (string.IsNullOrWhiteSpace(loggedUserNickname))
      {
        throw new ArgumentException("loggedUserNickname is required");
      }

      if (request == null)
      {
        throw new ArgumentNullException(nameof(request));
      }

      return employeeBenefitRepository.assignBenefit(
        request, loggedUserNickname);
    }
  }
}
