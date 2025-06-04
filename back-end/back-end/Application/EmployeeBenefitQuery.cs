using back_end.Repositories;
using back_end.Models;

namespace back_end.Application
{
  public class EmployeeBenefitQuery : IBenefitQuery<EmployeeBenefitDTO>
  {
    private readonly EmployeeBenefitRepository employeeBenefitRepository;

    public EmployeeBenefitQuery(EmployeeBenefitRepository
      employeeBenefitRepository)
    {
      this.employeeBenefitRepository = employeeBenefitRepository;
    }

    public List<EmployeeBenefitDTO> getBenefits(string loggedUserNickname)
    {
      List<EmployeeBenefitDTO> benefits;

      if (string.IsNullOrWhiteSpace(loggedUserNickname))
      {
        benefits = new List<EmployeeBenefitDTO>();
      } else
      {
        benefits = employeeBenefitRepository.getBenefits(loggedUserNickname);

        if (benefits == null)
        {
          benefits = new List<EmployeeBenefitDTO>();
        }
      }
      return benefits;
    }
    
    public EmployeeBenefitDTO getBenefitById(Guid id)
    {
      throw new NotImplementedException("This method is not implemented for EmployeeBenefitQuery.");
    }
  }
}
