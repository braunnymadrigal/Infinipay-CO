using back_end.Domain;

namespace back_end.Application
{
    public class WeeklyGrossSalaryComputation : IStrategyGrossSalaryComputation
    {
        public List<GrossSalaryModel> ComputeGrossSalary(List<GrossSalaryModel> grossSalaries, DateOnly startDate, DateOnly endDate)
        {
            foreach (var grossSalary in grossSalaries)
            {
                grossSalary.GrossSalary = grossSalary.GrossSalary * grossSalary.HoursWorked;
            }
            return grossSalaries;
        }
    }
}
