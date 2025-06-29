using back_end.Domain;

namespace back_end.Application
{
    public interface IContextGrossSalaryComputation
    {
        void setStrategy(IStrategyGrossSalaryComputation strategy);
        List<PayrollEmployeeModel> computeGrossSalary(List<PayrollEmployeeModel> 
            grossSalaries, DateOnly startDate, DateOnly endDate);
    }
}
