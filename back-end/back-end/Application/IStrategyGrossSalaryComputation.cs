using back_end.Domain;

namespace back_end.Application
{
    public interface IStrategyGrossSalaryComputation
    {
        List<GrossSalaryModel> ComputeGrossSalary(List<GrossSalaryModel> grossSalaries, DateOnly startDate, DateOnly endDate);
    }
}
