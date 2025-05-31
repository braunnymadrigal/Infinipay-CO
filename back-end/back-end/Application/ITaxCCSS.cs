using back_end.Domain;

namespace back_end.Application
{
    public interface ITaxCCSS
    {
        List<TaxCCSSModel> ComputeTaxesCCSS(List<GrossSalaryModel> grossSalaries);
    }
}
