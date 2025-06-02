using back_end.Domain;

namespace back_end.Application
{
    public interface IDeduction
    {
        Task<List<PayrollEmployeeModel>> computeDeductions(List<PayrollEmployeeModel> payrollEmployees);
    }
}
