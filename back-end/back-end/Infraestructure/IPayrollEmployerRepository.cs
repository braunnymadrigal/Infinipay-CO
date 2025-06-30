using back_end.Domain;

namespace back_end.Infraestructure
{
    public interface IPayrollEmployerRepository
    {
        PayrollEmployerModel getPayrollEmployer(PayrollEmployerModel payrollEmployer);
        DateOnly getDateOfLatestPayroll(string companyId);
    }
}
