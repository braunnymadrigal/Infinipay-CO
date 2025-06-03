using back_end.Domain;
public interface IRentTax
{
  List<PayrollEmployeeModel> calculateRentTaxes(List<PayrollEmployeeModel>
    payrollEmployees, DateOnly endDate);
}