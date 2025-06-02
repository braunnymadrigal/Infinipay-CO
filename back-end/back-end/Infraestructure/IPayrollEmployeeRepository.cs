using back_end.Domain;

namespace back_end.Infraestructure
{
    public interface IPayrollEmployeeRepository
    {
        List<PayrollEmployeeModel> getPayrollEmployees(string employerId, DateOnly startDate, DateOnly endDate);
    }
}
