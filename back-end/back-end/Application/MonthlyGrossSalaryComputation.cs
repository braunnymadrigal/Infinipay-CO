using back_end.Domain;

namespace back_end.Application
{
    public class MonthlyGrossSalaryComputation : IStrategyGrossSalaryComputation
    {
        private const int MAXIMUM_DAYS_OF_WORK = 30;

        public List<GrossSalaryModel> ComputeGrossSalary(List<GrossSalaryModel> grossSalaries, DateOnly startDate, DateOnly endDate)
        {
            foreach (var grossSalary in grossSalaries)
            {
                if (grossSalary.HiringDate > startDate)
                {
                    var numberOfWorkedDays = (endDate.Day - grossSalary.HiringDate.Day) + 1;
                    var newGrossSalary = (grossSalary.GrossSalary / MAXIMUM_DAYS_OF_WORK) * numberOfWorkedDays;
                    grossSalary.GrossSalary = newGrossSalary;
                }
            }
            return grossSalaries;
        }
    }
}
