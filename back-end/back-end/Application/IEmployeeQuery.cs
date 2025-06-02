using back_end.Domain;

namespace back_end.Application
{
  public interface IEmployeeQuery
  {
    public EmployeeModel GetEmployee(Guid id);
  }
}