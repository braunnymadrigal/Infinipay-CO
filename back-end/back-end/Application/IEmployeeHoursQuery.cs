using back_end.Domain;

namespace back_end.Application
{
  public interface IEmployeeHoursQuery
  {
    public EmployeeHoursModel getEmployeeHoursContract(string loggedUserId);
    public List<HoursModel> getEmployeeHoursList(string loggedUserId,
      DateOnly startDate, DateOnly endDate);
  }
}
