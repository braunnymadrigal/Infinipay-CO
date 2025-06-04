using back_end.Domain;

namespace back_end.Application
{
    public interface ITaxCCSS
    {
        List<PayrollEmployeeModel> computeTaxesCCSS(List<PayrollEmployeeModel> payrollEmployees, 
            DateOnly endDate);
    }
}
