using back_end.Domain;

namespace back_end.Application
{
    public interface IPayrollEmployee
    {
        List<PayrollEmployeeModel> getPayrollEmployees(PayrollEmployerModel payrollEmployer);
    }
}
