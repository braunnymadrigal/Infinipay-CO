using AutoFixture;
using back_end.Application;
using back_end.Domain;

namespace Tests
{
    public class RentTaxTest
    {
        private const string HIRING_TYPE_EXCLUDED_FROM_RENT_TAX = "servicios";

        private Fixture _fixture;
        private IRentTax _rentTax;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _rentTax = new RentTax();
        }

        [Test]
        public void CalculateRentTaxes_DoesNotCalculateTax_WhenHiringTypeIsExcluded()
        {
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.birthDate, DateOnly.MinValue)
                .With(m => m.hiringDate, DateOnly.MinValue)
                .With(m => m.hiringType, HIRING_TYPE_EXCLUDED_FROM_RENT_TAX)
                .With(m => m.computedGrossSalary, 1_000_000)
                .With(m => m.taxes, new PayrollTaxModel { employeeRent = 0 })
                .Without(m => m.previousComputedGrossSalaries)
                .CreateMany(1)
                .ToList();

            var result = _rentTax.calculateRentTaxes(employees, new DateOnly(2025, 6, 30));

            Assert.That(result[0].taxes.employeeRent, Is.EqualTo(0));
        }

        [Test]
        public void CalculateRentTaxes_DoesNotCalculateTax_WhenSalaryBelowTier1()
        {
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.birthDate, DateOnly.MinValue)
                .With(m => m.hiringDate, DateOnly.MinValue)
                .With(m => m.hiringType, "tiempoCompleto")
                .With(m => m.computedGrossSalary, 900_000)
                .With(m => m.taxes, new PayrollTaxModel { employeeRent = 0 })
                .Without(m => m.previousComputedGrossSalaries)
                .CreateMany(1)
                .ToList();

            var result = _rentTax.calculateRentTaxes(employees, new DateOnly(2025, 6, 30));

            Assert.That(result[0].taxes.employeeRent, Is.EqualTo(0));
        }

        [Test]
        public void CalculateRentTaxes_ReturnsPositiveTax_WhenSalaryAboveTier1()
        {
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.birthDate, DateOnly.MinValue)
                .With(m => m.hiringDate, DateOnly.MinValue)
                .With(m => m.hiringType, "medioTiempo")
                .With(m => m.computedGrossSalary, 1_400_000)
                .With(m => m.taxes, new PayrollTaxModel { employeeRent = 0 })
                .Without(m => m.previousComputedGrossSalaries)
                .CreateMany(1)
                .ToList();

            var result = _rentTax.calculateRentTaxes(employees, new DateOnly(2025, 6, 30));

            Assert.That(result[0].taxes.employeeRent, Is.GreaterThan(0));
        }

        [Test]
        public void CalculateRentTaxes_ReturnsPositiveTax_WhenPartialMonth()
        {
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.birthDate, DateOnly.MinValue)
                .With(m => m.hiringDate, DateOnly.MinValue)
                .With(m => m.hiringType, "horas")
                .With(m => m.computedGrossSalary, 1_200_000)
                .With(m => m.taxes, new PayrollTaxModel { employeeRent = 0 })
                .Without(m => m.previousComputedGrossSalaries)
                .CreateMany(1)
                .ToList();

            var result = _rentTax.calculateRentTaxes(employees, new DateOnly(2025, 6, 15));

            Assert.That(result[0].taxes.employeeRent, Is.GreaterThan(0));
        }

        [Test]
        public void CalculateRentTaxes_DoesNotCalculateTax_WhenSalaryAtTier1Limit()
        {
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.birthDate, DateOnly.MinValue)
                .With(m => m.hiringDate, DateOnly.MinValue)
                .With(m => m.hiringType, "tiempoCompleto")
                .With(m => m.computedGrossSalary, 922_000)
                .With(m => m.taxes, new PayrollTaxModel { employeeRent = 0 })
                .Without(m => m.previousComputedGrossSalaries)
                .CreateMany(1)
                .ToList();

            var result = _rentTax.calculateRentTaxes(employees, new DateOnly(2025, 6, 30));

            Assert.That(result[0].taxes.employeeRent, Is.EqualTo(0));
        }
    }
}