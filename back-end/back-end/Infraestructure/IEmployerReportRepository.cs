using back_end.Domain;

namespace back_end.Infraestructure
{
  public interface IEmployerReportRepository
  {
    EmployerPayrollReport GetEmployerPayrollReport(string employerId);
  }
}