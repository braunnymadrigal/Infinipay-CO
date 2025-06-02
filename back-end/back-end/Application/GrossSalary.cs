using System.Collections.Generic;
using back_end.Domain;
using back_end.Infraestructure;

namespace back_end.Application
{
    public class GrossSalary : IGrossSalary
    {
        private const int WEEKLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK = 7;
        private const int BIWEEKLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK = 15;
        private const int MONTHLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK = 30;

        private int numberOfWorkedDays;
        private IContextGrossSalaryComputation contextGrossSalaryComputation;

        public GrossSalary(IContextGrossSalaryComputation contextGrossSalaryComputation)
        {
            this.contextGrossSalaryComputation = contextGrossSalaryComputation;
        }

        public List<PayrollEmployeeModel> computeAllGrossSalaries(List<PayrollEmployeeModel> 
            payrollEmployees, DateOnly startDate, DateOnly endDate)
        {
            setNumberOfWorkedDays(startDate, endDate);
            SetGrossSalaryComputationStrategy();
            payrollEmployees = contextGrossSalaryComputation.ComputeGrossSalary(payrollEmployees, 
                startDate, endDate);
            return payrollEmployees;
        }

        private void SetGrossSalaryComputationStrategy()
        {
            switch (numberOfWorkedDays)
            {
                case WEEKLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK:
                    contextGrossSalaryComputation.SetStrategy(new WeeklyGrossSalaryComputation());
                    break;
                case BIWEEKLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK:
                    contextGrossSalaryComputation.SetStrategy(new BiweeklyGrossSalaryComputation());
                    break;
                case MONTHLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK:
                    contextGrossSalaryComputation.SetStrategy(new MonthlyGrossSalaryComputation());
                    break;
                default:
                    throw new Exception("Improper Strategy is tried to be set");
            }
        }

        private void setNumberOfWorkedDays(DateOnly startDate, DateOnly endDate)
        {
            var rawNumberOfDays = endDate.DayNumber - startDate.DayNumber;
            numberOfWorkedDays = rawNumberOfDays <= BIWEEKLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK
                ? BIWEEKLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK : MONTHLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK;
            if (rawNumberOfDays <= WEEKLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK)
            {
                numberOfWorkedDays = WEEKLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK;
            }
        }
    }
}
