using back_end.Domain;

namespace back_end.Application
{
    public interface IStrategyGrossSalaryComputation
    {
        List<PayrollEmployeeModel> ComputeGrossSalary(List<PayrollEmployeeModel> grossSalaries, DateOnly startDate, DateOnly endDate);
    }
}
