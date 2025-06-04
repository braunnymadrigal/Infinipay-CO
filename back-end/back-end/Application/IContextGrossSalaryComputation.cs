using back_end.Domain;

namespace back_end.Application
{
    public interface IContextGrossSalaryComputation
    {
        void SetStrategy(IStrategyGrossSalaryComputation strategy);
        List<PayrollEmployeeModel> ComputeGrossSalary(List<PayrollEmployeeModel> 
            grossSalaries, DateOnly startDate, DateOnly endDate);
    }
}
