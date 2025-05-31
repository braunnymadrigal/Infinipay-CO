using back_end.Domain;

namespace back_end.Application
{
    public class BiweeklyGrossSalaryComputation : IStrategyGrossSalaryComputation
    {
        private const int NUMBER_OF_DIVISOR = 2;
        private const int MAXIMUM_DAYS_OF_WORK = 15;

        public List<GrossSalaryModel> ComputeGrossSalary(List<GrossSalaryModel> grossSalaries, DateOnly startDate, DateOnly endDate)
        {
            foreach (var grossSalary in grossSalaries)
            {
                grossSalary.ComputedGrossSalary = grossSalary.GrossSalary / NUMBER_OF_DIVISOR;
                if (grossSalary.HiringDate > startDate)
                {
                    var numberOfWorkedDays = (endDate.Day - grossSalary.HiringDate.Day) + 1;
                    var newGrossSalary = (grossSalary.GrossSalary / MAXIMUM_DAYS_OF_WORK) * numberOfWorkedDays;
                    grossSalary.ComputedGrossSalary = newGrossSalary;
                }
            }
            return grossSalaries;
        }
    }
}
