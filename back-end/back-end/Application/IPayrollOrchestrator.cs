using back_end.Domain;
using back_end.Models;

namespace back_end.Application
{
  public interface IPayrollOrchestrator
  {
    Task<List<EmployeePayrollResult>> ComputePayrollAsync(string employerId
      , DateOnly start, DateOnly end);
  }
}
