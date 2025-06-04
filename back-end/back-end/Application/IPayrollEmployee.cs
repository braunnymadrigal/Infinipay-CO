using back_end.Domain;

namespace back_end.Application
{
    public interface IPayrollEmployee
    {
        List<PayrollEmployeeModel> getPayrollEmployees(string employerId,
            DateOnly startDate, DateOnly endDate);
    }
}
