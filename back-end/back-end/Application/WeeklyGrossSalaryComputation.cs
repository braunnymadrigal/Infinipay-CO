using back_end.Domain;

namespace back_end.Application
{
    public class WeeklyGrossSalaryComputation : IStrategyGrossSalaryComputation
    {
        public List<GrossSalaryModel> ComputeGrossSalary(List<GrossSalaryModel> grossSalaries, DateOnly startDate, DateOnly endDate)
        {
            for (int i = 0; i < grossSalaries.Count; ++i)
            {
                if (grossSalaries[i].HiringDate > endDate)
                {
                    grossSalaries[i].EmployeeId = "";
                }
                else
                {
                    grossSalaries[i].GrossSalary = grossSalaries[i].GrossSalary * grossSalaries[i].HoursWorked;
                }
            }
            return grossSalaries;
        }
}
}
