using back_end.Domain;
using back_end.Infraestructure;

namespace back_end.Application
{
    public class PayrollEmployer : IPayrollEmployer
    {
        private readonly IPayrollEmployerRepository _payrollEmployerRepository;

        public PayrollEmployer(IPayrollEmployerRepository payrollEmployerRepository)
        {
            _payrollEmployerRepository = payrollEmployerRepository;
        }

        public PayrollEmployerModel getPayrollEmployer(string id, DateOnly startDate, DateOnly endDate)
        {
            return new PayrollEmployerModel();
        }

        public string getPaymentType(string id)
        {
            var payrollEmployer = new PayrollEmployerModel
            {
                id = id,
            };
            payrollEmployer.id = id;
            payrollEmployer = _payrollEmployerRepository.getPayrollEmployer(payrollEmployer);
            checkStringNotEmpty(payrollEmployer.paymentType);
            return payrollEmployer.paymentType;
        }

        private void checkStringNotEmpty(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception("PayrollEmployer: caught invalid string.");
            }
        }
    }
}
