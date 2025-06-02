using back_end.Domain;

namespace back_end.Application
{
    public interface IGrossSalary
    {
        List<PayrollEmployeeModel> computeAllGrossSalaries(List<PayrollEmployeeModel> payrollEmployees
            , DateOnly startDate, DateOnly endDate);
    }
}
