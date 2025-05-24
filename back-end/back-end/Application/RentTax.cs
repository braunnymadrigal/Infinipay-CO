using back_end.Infraestructure;
namespace back_end.Application
{
  public class RentTax
  {
    private readonly RentTaxRepository _repository;
    private readonly IRentTax _taxStrategy;
    public RentTax(RentTaxRepository repository,
      IRentTax taxStrategy)
    {
      _repository = repository;
      _taxStrategy = taxStrategy;
    }
    public decimal CalculateRentTax(string employeeId)
    {
      var (contractType, grossSalary) =
        _repository.GetEmployeeSalaryInfo(employeeId);

      decimal estimatedGrossSalary;

      switch (contractType)
      {
        case "mensual":
          estimatedGrossSalary = grossSalary;
          break;

        case "quincenal":
          estimatedGrossSalary = grossSalary;
          break;

        case "semanal":
          estimatedGrossSalary = grossSalary;
          // TODO: lógica de cálculo de salario semanal
          break;

        default:
          throw new Exception("Tipo de contrato no reconocido.");
      }
      return _taxStrategy.CalculateRentTax(estimatedGrossSalary);
    }
  }
}
