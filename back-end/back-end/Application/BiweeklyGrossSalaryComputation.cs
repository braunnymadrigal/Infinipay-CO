using back_end.Domain;

namespace back_end.Application
{
    public class BiweeklyGrossSalaryComputation : IStrategyGrossSalaryComputation
    {
        private const int NUMBER_OF_DIVISOR = 2;
        private const int MAXIMUM_DAYS_OF_WORK = 15;

        public List<PayrollEmployeeModel> ComputeGrossSalary(List<PayrollEmployeeModel> payrollEmployees, DateOnly startDate, DateOnly endDate)
        {
            foreach (var payrollEmployee in payrollEmployees)
            {
                payrollEmployee.computedGrossSalary = payrollEmployee.rawGrossSalary / NUMBER_OF_DIVISOR;
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
