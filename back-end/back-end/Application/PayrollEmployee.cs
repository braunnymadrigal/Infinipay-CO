using back_end.Domain;
using back_end.Infraestructure;

namespace back_end.Application
{
    public class PayrollEmployee : IPayrollEmployee
    {
        private const int MAXIMUM_NUMBER_OF_DAYS_A_DATE_RANGE_CAN_REPRESENT = 31;

        private readonly IPayrollEmployeeRepository payrollEmployeeRepository;

        public PayrollEmployee(IPayrollEmployeeRepository payrollEmployeeRepository)
        {
            this.payrollEmployeeRepository = payrollEmployeeRepository;
        }

        public List<PayrollEmployeeModel> getPayrollEmployees(string employerId,
            DateOnly startDate, DateOnly endDate)
        {
            checkDateRangeCorrectness(startDate, endDate);
            var payrollEmployees = payrollEmployeeRepository.getPayrollEmployees(employerId, 
                startDate, endDate);
            return payrollEmployees;
        }

        private void checkDateRangeCorrectness(DateOnly startDate, DateOnly endDate)
        {
            if (startDate == DateOnly.MinValue || endDate == DateOnly.MaxValue)
            {
                throw new Exception("Date values are not coherent.");
            }
            if (startDate >= endDate)
            {
                throw new Exception("The start date shall not surpass the end date.");
            }
            if (startDate.AddDays(MAXIMUM_NUMBER_OF_DAYS_A_DATE_RANGE_CAN_REPRESENT) < endDate)
            {
                throw new Exception("The range of date shall not represent more than one month");
            }
        }
    }
}
