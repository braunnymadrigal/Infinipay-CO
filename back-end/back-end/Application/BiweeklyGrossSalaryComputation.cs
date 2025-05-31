using back_end.Domain;

namespace back_end.Application
{
    public class BiweeklyGrossSalaryComputation : IStrategyGrossSalaryComputation
    {
        private const int MAXIMUM_DAYS_OF_WORK = 15;

        public List<GrossSalaryModel> ComputeGrossSalary(List<GrossSalaryModel> grossSalaries, DateOnly startDate, DateOnly endDate)
        {
            for (int i = 0; i < grossSalaries.Count; ++i)
            {
                grossSalaries[i].GrossSalary = grossSalaries[i].GrossSalary / 2;
                if (grossSalaries[i].HiringDate > endDate)
                {
                    grossSalaries[i].EmployeeId = "";
                }
                else
                {
                    if (grossSalaries[i].HiringDate > startDate)
                    {
                        var numberOfWorkedDays = (endDate.Day - grossSalaries[i].HiringDate.Day) + 1;
                        var newGrossSalary = (grossSalaries[i].GrossSalary / MAXIMUM_DAYS_OF_WORK) * numberOfWorkedDays;
                        grossSalaries[i].GrossSalary = newGrossSalary;
                    }
                }
            }
            return grossSalaries;
        }
    }
}
