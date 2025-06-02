using back_end.Domain;

namespace back_end.Application
{
    public class MonthlyGrossSalaryComputation : IStrategyGrossSalaryComputation
    {
        private const int MAXIMUM_DAYS_OF_WORK = 30;

        public List<PayrollEmployeeModel> ComputeGrossSalary(List<PayrollEmployeeModel> payrollEmployees, 
            DateOnly startDate, DateOnly endDate)
        {
            foreach (var payrollEmployee in payrollEmployees)
            {
                payrollEmployee.computedGrossSalary = payrollEmployee.rawGrossSalary;
                if (payrollEmployee.hiringDate > startDate)
                {
                    var numberOfWorkedDays = (endDate.Day - payrollEmployee.hiringDate.Day) + 1;
                    var newGrossSalary = (payrollEmployee.rawGrossSalary / MAXIMUM_DAYS_OF_WORK) * numberOfWorkedDays;
                    payrollEmployee.computedGrossSalary = newGrossSalary;
                }
            }
            return payrollEmployees;
        }
    }
}
