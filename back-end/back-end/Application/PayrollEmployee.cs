using back_end.Domain;
using back_end.Infraestructure;

namespace back_end.Application
{
    public class PayrollEmployee : IPayrollEmployee
    {
        private readonly IPayrollEmployeeRepository _payrollEmployeeRepository;

        public PayrollEmployee(IPayrollEmployeeRepository payrollEmployeeRepository)
        {
            _payrollEmployeeRepository = payrollEmployeeRepository;
        }

        public List<PayrollEmployeeModel> getPayrollEmployees(PayrollEmployerModel payrollEmployer)
        {
            var payrollEmployees = _payrollEmployeeRepository.getPayrollEmployees(payrollEmployer);
            return payrollEmployees;
        }
    }
}
