using back_end.Domain;

namespace back_end.Application
{
  public interface IEmployeeHoursCommand
  {
    public bool RegisterEmployeeHours(string loggedUserId
      , List<HoursModel> employeeHoursWorked);
  }
}
