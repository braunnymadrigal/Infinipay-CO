using back_end.Domain;

namespace back_end.Application
{
    public interface IMandatoryTaxes
    {
        public List<PayrollEmployeeModel> calculateMandatoryTaxes(List<PayrollEmployeeModel> 
            payrollEmployees);
    }
}
