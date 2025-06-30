using back_end.Domain;

namespace back_end.Application
{
    public interface IStrategyGrossSalaryComputation
    {
        List<PayrollEmployeeModel> computeGrossSalary(List<PayrollEmployeeModel> grossSalaries, DateOnly startDate, DateOnly endDate);
    }
}
