using back_end.Domain;

namespace back_end.Application
{
    public class ContextGrossSalaryComputation : IContextGrossSalaryComputation
    {
        private IStrategyGrossSalaryComputation? strategy;

        public void SetStrategy(IStrategyGrossSalaryComputation strategy)
        {
            this.strategy = strategy;
        }

        public List<PayrollEmployeeModel> ComputeGrossSalary(List<PayrollEmployeeModel> payrollEmployees,
            DateOnly startDate, DateOnly endDate)
        {
            HandleErrorsProvokedByBadInitialization();
            return (strategy.ComputeGrossSalary(payrollEmployees, startDate, endDate));
        }

        private void HandleErrorsProvokedByBadInitialization()
        {
            if (strategy == null)
            {
                throw new Exception("ContextGrossSalaryComputation has not been setted correctly.");
            }
        }
    }
}
