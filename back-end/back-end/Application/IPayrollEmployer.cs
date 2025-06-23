using back_end.Domain;

namespace back_end.Application
{
    public interface IPayrollEmployer
    {
        PayrollEmployerModel getPayrollEmployer(string id, DateOnly startDate, DateOnly endDate);
        string getPaymentType(string id);
    }
}
