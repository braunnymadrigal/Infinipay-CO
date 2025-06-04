using back_end.Domain;

namespace back_end.Application
{
  public interface IEmployeeHoursCommand
  {
    public bool registerEmployeeHours(string loggedUserId
      , List<HoursModel> employeeHoursWorked);
  }
}
