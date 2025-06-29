using back_end.Domain;

namespace back_end.Application
{
    public interface IDeduction
    {
        Task<List<PayrollEmployeeModel>> calculateDeductions(List<PayrollEmployeeModel> 
            payrollEmployees, string paymentType);
    }
}
