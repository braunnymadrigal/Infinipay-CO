using back_end.Domain;

namespace back_end.Infraestructure
{
  public interface IEmployeeHoursRepository
  {
    public EmployeeHoursModel getEmployeeHoursContract(string loggedUserId);
    public List<HoursModel> getEmployeeHoursList(string loggedUserId
      , DateOnly startDate, DateOnly endDate);

    public bool registerEmployeeHours(string loggedUserId
      , List<HoursModel> employeeHoursWorked);
  }
}
