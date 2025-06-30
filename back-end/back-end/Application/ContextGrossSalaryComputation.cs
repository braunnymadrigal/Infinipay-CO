using back_end.Domain;

namespace back_end.Application
{
    public class ContextGrossSalaryComputation : IContextGrossSalaryComputation
    {
        private IStrategyGrossSalaryComputation? _strategy;

        public void setStrategy(IStrategyGrossSalaryComputation strategy)
        {
           _strategy = strategy;
        }

        public List<PayrollEmployeeModel> computeGrossSalary(List<PayrollEmployeeModel> payrollEmployees,
            DateOnly startDate, DateOnly endDate)
        {
            handleErrorsProvokedByBadInitialization();
            return (_strategy.computeGrossSalary(payrollEmployees, startDate, endDate));
        }

        private void handleErrorsProvokedByBadInitialization()
        {
            if (_strategy == null)
            {
                throw new Exception("ContextGrossSalaryComputation: Strategy is null.");
            }
        }
    }
}
