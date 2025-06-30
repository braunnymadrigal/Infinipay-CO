using back_end.Domain;

namespace back_end.Infraestructure
{
  public interface IEmployeeReportRepository
  {
    public EmployeeFullReport GetEmployeePayrollReport(string employeeId);
  }
}