using back_end.Domain;

namespace back_end.Application
{
    public class ContextGrossSalaryComputation : IContextGrossSalaryComputation
    {
        private DateOnly startDate;
        private DateOnly endDate;
        private IStrategyGrossSalaryComputation? strategy;

        public void SetStrategy(IStrategyGrossSalaryComputation strategy)
        {
            this.strategy = strategy;
        }

        public void SetRangeOfDates(DateOnly startDate, DateOnly endDate)
        {
            this.startDate = startDate;
            this.endDate = endDate;
        }

        public List<GrossSalaryModel> ComputeGrossSalary(List<GrossSalaryModel> grossSalaries)
        {
            HandleErrorsProvokedByBadInitialization();
            return (strategy.ComputeGrossSalary(grossSalaries, startDate, endDate));
        }

        private void HandleErrorsProvokedByBadInitialization()
        {
            if (strategy == null || startDate == DateOnly.MinValue || endDate == DateOnly.MaxValue)
            {
                throw new Exception("ContextGrossSalaryComputation has not been setted correctly.");
            }
        }
    }
}
