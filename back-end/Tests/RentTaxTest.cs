using back_end.Application;
using back_end.Domain;

namespace UnitTestingRentTax
{
  public class RentTaxTest
  {
    private RentTax _rentTax;

    [SetUp]
    public void Setup()
    {
      _rentTax = new RentTax();
    }

    [Test]
    public void ExcludedEmployee_ShouldHaveZeroRentTax()
    {
      var employee = new PayrollEmployeeModel
      {
        hiringType = "servicios",
        computedGrossSalary = 1_000_000,
        previousComputedGrossSalaries = 
        new List<PayrollPreviousComputedGrossSalary>()
      };

      var result =
        _rentTax.calculateRentTaxes(new List<PayrollEmployeeModel> { employee }
        , new DateOnly(2025, 6, 30));

      Assert.That(result[0].rentTax, Is.EqualTo(0));
    }

    [Test]
    public void EmployeeBelowTier1_ShouldHaveZeroRentTax()
    {
      var employee = new PayrollEmployeeModel
      {
        hiringType = "tiempoCompleto",
        computedGrossSalary = 900_000,
        previousComputedGrossSalaries =
        new List<PayrollPreviousComputedGrossSalary>()
      };

      var result =
        _rentTax.calculateRentTaxes(new List<PayrollEmployeeModel> { employee }
        , new DateOnly(2025, 6, 30));

      Assert.That(result[0].rentTax, Is.EqualTo(0));
    }

    [Test]
    public void EmployeeAboveTier1_ShouldHaveCorrectRentTax()
    {
      var employee = new PayrollEmployeeModel
      {
        hiringType = "medioTiempo",
        computedGrossSalary = 1_400_000,
        previousComputedGrossSalaries =
        new List<PayrollPreviousComputedGrossSalary>()
      };

      var result =
        _rentTax.calculateRentTaxes(new List<PayrollEmployeeModel> { employee }
        , new DateOnly(2025, 6, 30));

      Assert.That(result[0].rentTax, Is.GreaterThan(0));
    }

    [Test]
    public void PartialMonthCalculation_ShouldProjectTaxCorrectly()
    {
      var employee = new PayrollEmployeeModel
      {
        hiringType = "horas",
        computedGrossSalary = 1_200_000,
        previousComputedGrossSalaries =
        new List<PayrollPreviousComputedGrossSalary>()
      };

      var result =
        _rentTax.calculateRentTaxes(new List<PayrollEmployeeModel> { employee }
        , new DateOnly(2025, 6, 15));

      Assert.That(result[0].rentTax, Is.GreaterThan(0));
    }

    [Test]
    public void EndOfMonth_ShouldUseAccumulatedSalaries()
    {
      var employee = new PayrollEmployeeModel
      {
        hiringType = "tiempoCompleto",
        computedGrossSalary = 2_000_000,
        previousComputedGrossSalaries =
          new List<PayrollPreviousComputedGrossSalary>
          {
            new PayrollPreviousComputedGrossSalary
            {
              amount = 2_000_000,
              startDate = new DateOnly(2025, 5, 1)
            }
          }
      };

      var result =
        _rentTax.calculateRentTaxes(new List<PayrollEmployeeModel> { employee }
        , new DateOnly(2025, 6, 30));

      Assert.That(result[0].rentTax, Is.GreaterThan(0));
    }
  }
}
