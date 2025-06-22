using back_end.Domain;
using back_end.Infraestructure;

namespace back_end.Application
{
    public class PayrollEmployee : IPayrollEmployee
    {
        private readonly IPayrollEmployeeRepository payrollEmployeeRepository;

        public PayrollEmployee(IPayrollEmployeeRepository payrollEmployeeRepository)
        {
            this.payrollEmployeeRepository = payrollEmployeeRepository;
        }

        public List<PayrollEmployeeModel> getPayrollEmployees(PayrollEmployerModel payrollEmployer)
        {
            var payrollEmployees = payrollEmployeeRepository.getPayrollEmployees(payrollEmployer);
            return payrollEmployees;
        }
    }
}
