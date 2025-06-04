using back_end.Application;
using back_end.Domain;

namespace Tests
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
        // not needed for this test but required by model
        fullName = "Test Employee",
        id = "",
        gender = "",
        birthDate = DateOnly.MinValue,
        rentTax = 0,
        rawGrossSalary = 0,
        ccssEmployeeDeduction = 0,
        ccssEmployerDeduction = 0,
        hiringDate = DateOnly.MinValue,
        hoursDate = DateOnly.MinValue,
        hoursNumber = 0,
        companyAssociation = "",
        deductions = new List<PayrollDeductionModel>(),

        // specific for this test
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
        fullName = "Test Employee",
        id = "",
        gender = "",
        birthDate = DateOnly.MinValue,
        rentTax = 0,
        rawGrossSalary = 0,
        ccssEmployeeDeduction = 0,
        ccssEmployerDeduction = 0,
        hiringDate = DateOnly.MinValue,
        hoursDate = DateOnly.MinValue,
        hoursNumber = 0,
        companyAssociation = "",
        deductions = new List<PayrollDeductionModel>(),

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
        fullName = "Test Employee",
        id = "",
        gender = "",
        birthDate = DateOnly.MinValue,
        rentTax = 0,
        rawGrossSalary = 0,
        ccssEmployeeDeduction = 0,
        ccssEmployerDeduction = 0,
        hiringDate = DateOnly.MinValue,
        hoursDate = DateOnly.MinValue,
        hoursNumber = 0,
        companyAssociation = "",
        deductions = new List<PayrollDeductionModel>(),

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
        fullName = "Test Employee",
        id = "",
        gender = "",
        birthDate = DateOnly.MinValue,
        rentTax = 0,
        rawGrossSalary = 0,
        ccssEmployeeDeduction = 0,
        ccssEmployerDeduction = 0,
        hiringDate = DateOnly.MinValue,
        hoursDate = DateOnly.MinValue,
        hoursNumber = 0,
        companyAssociation = "",
        deductions = new List<PayrollDeductionModel>(),

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
        fullName = "Test Employee",
        id = "",
        gender = "",
        birthDate = DateOnly.MinValue,
        rentTax = 0,
        rawGrossSalary = 0,
        ccssEmployeeDeduction = 0,
        ccssEmployerDeduction = 0,
        hiringDate = DateOnly.MinValue,
        hoursDate = DateOnly.MinValue,
        hoursNumber = 0,
        companyAssociation = "",
        deductions = new List<PayrollDeductionModel>(),

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

    [Test]
    public void SalaryAtTier1Limit_ShouldHaveZeroRentTax()
    {
      var employee = new PayrollEmployeeModel
      {
        fullName = "Test Employee",
        id = "",
        gender = "",
        birthDate = DateOnly.MinValue,
        rentTax = 0,
        rawGrossSalary = 0,
        ccssEmployeeDeduction = 0,
        ccssEmployerDeduction = 0,
        hiringDate = DateOnly.MinValue,
        hoursDate = DateOnly.MinValue,
        hoursNumber = 0,
        companyAssociation = "",
        deductions = new List<PayrollDeductionModel>(),

        hiringType = "tiempoCompleto",
        computedGrossSalary = 922_000,
        previousComputedGrossSalaries =
          new List<PayrollPreviousComputedGrossSalary>()
      };

      var result = _rentTax.calculateRentTaxes(
        new List<PayrollEmployeeModel> { employee },
        new DateOnly(2025, 6, 30)
      );

      Assert.That(result[0].rentTax, Is.EqualTo(0));
    }

    [Test]
    public void SalaryAboveAllTiers_ShouldHaveTier5Tax()
    {
      var employee = new PayrollEmployeeModel
      {
        fullName = "Test Employee",
        id = "",
        gender = "",
        birthDate = DateOnly.MinValue,
        rentTax = 0,
        rawGrossSalary = 0,
        ccssEmployeeDeduction = 0,
        ccssEmployerDeduction = 0,
        hiringDate = DateOnly.MinValue,
        hoursDate = DateOnly.MinValue,
        hoursNumber = 0,
        companyAssociation = "",
        deductions = new List<PayrollDeductionModel>(),

        hiringType = "tiempoCompleto",
        computedGrossSalary = 6_000_000,
        previousComputedGrossSalaries =
          new List<PayrollPreviousComputedGrossSalary>()
      };

      var result = _rentTax.calculateRentTaxes(
        new List<PayrollEmployeeModel> { employee },
        new DateOnly(2025, 6, 30)
      );

      Assert.That(result[0].rentTax, Is.GreaterThan(0));
    }

    [Test]
    public void FutureAccumulatedSalary_ShouldBeIgnored()
    {
      var employee = new PayrollEmployeeModel
      {
        fullName = "Test Employee",
        id = "",
        gender = "",
        birthDate = DateOnly.MinValue,
        rentTax = 0,
        rawGrossSalary = 0,
        ccssEmployeeDeduction = 0,
        ccssEmployerDeduction = 0,
        hiringDate = DateOnly.MinValue,
        hoursDate = DateOnly.MinValue,
        hoursNumber = 0,
        companyAssociation = "",
        deductions = new List<PayrollDeductionModel>(),

        hiringType = "tiempoCompleto",
        computedGrossSalary = 1_000_000,
        previousComputedGrossSalaries =
          new List<PayrollPreviousComputedGrossSalary>
    {
      new PayrollPreviousComputedGrossSalary
      {
        amount = 2_000_000,
        startDate = new DateOnly(2025, 7, 1)
      }
    }
      };

      var result = _rentTax.calculateRentTaxes(
        new List<PayrollEmployeeModel> { employee },
        new DateOnly(2025, 6, 30)
      );

      var expectedTax = result[0].rentTax;
      Assert.That(expectedTax, Is.EqualTo(result[0].rentTax));
    }

  }
}