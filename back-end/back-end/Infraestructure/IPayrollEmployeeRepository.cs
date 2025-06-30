using back_end.Domain;

namespace back_end.Infraestructure
{
    public interface IPayrollEmployeeRepository
    {
        List<PayrollEmployeeModel> getPayrollEmployees(PayrollEmployerModel payrollEmployer);
    }
}
