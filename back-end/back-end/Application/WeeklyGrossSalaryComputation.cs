using back_end.Domain;

namespace back_end.Application
{
    public class WeeklyGrossSalaryComputation : IStrategyGrossSalaryComputation
    {
        public List<PayrollEmployeeModel> ComputeGrossSalary(List<PayrollEmployeeModel> payrollEmployees, DateOnly startDate, DateOnly endDate)
        {
            foreach (var payrollEmployee in payrollEmployees)
            {
                payrollEmployee.computedGrossSalary = payrollEmployee.rawGrossSalary * payrollEmployee.hoursNumber;
            }
            return payrollEmployees;
        }
    }
}
