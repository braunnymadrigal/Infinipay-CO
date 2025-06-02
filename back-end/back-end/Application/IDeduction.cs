using back_end.Domain;

namespace back_end.Application
{
    public interface IDeduction
    {
        List<PayrollEmployeeModel> computeDeductions(List<PayrollEmployeeModel> payrollEmployees);
    }
}
