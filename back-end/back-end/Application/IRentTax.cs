using back_end.Domain;
public interface IRentTax
{
  List<RentTaxModel> calculateRentTaxes(List<GrossSalaryModel> grossSalaries);
}