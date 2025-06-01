using back_end.Domain;

namespace back_end.Infraestructure
{
  public interface IEmployeeHoursRepository
  {
    public EmployeeHoursModel GetEmployeeHoursContract(string loggedUserId);
    public List<HoursModel> GetEmployeeHoursList(string loggedUserId
      , DateOnly startDate, DateOnly endDate);

    public bool RegisterEmployeeHours(string loggedUserId
      , List<HoursModel> employeeHoursWorked);
  }
}
