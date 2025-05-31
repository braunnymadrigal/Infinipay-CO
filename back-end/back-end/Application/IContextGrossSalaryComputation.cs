using back_end.Domain;

namespace back_end.Application
{
    public interface IContextGrossSalaryComputation
    {
        void SetStrategy(IStrategyGrossSalaryComputation strategy);
        void SetRangeOfDates(DateOnly startDate, DateOnly endDate);
        List<GrossSalaryModel> ComputeGrossSalary(List<GrossSalaryModel> grossSalaries);
    }
}
