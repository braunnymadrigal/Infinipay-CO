using back_end.Domain;

namespace back_end.Application
{
    public interface ITaxCCSS
    {
        List<PayrollEmployeeModel> ComputeTaxesCCSS(List<PayrollEmployeeModel> payrollEmployees);
    }
}
