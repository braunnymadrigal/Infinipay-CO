using back_end.Application;
using back_end.Domain;
using AutoFixture;

namespace Tests
{
    class MandatoryTaxesTest
    {
        private const string HIRING_TYPE_EXCLUDED_FROM_MANDATORY_TAXES = "servicios";

        private Fixture _fixture;
        private IMandatoryTaxes _mandatoryTaxes;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _mandatoryTaxes = new MandatoryTaxes();
        }

        [Test]
        public void CalculateMandatoryTaxes_DoesNotCalculateTaxes_WhenHiringTypeIsExcluded()
        {
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.birthDate, DateOnly.MinValue)
                .With(m => m.hiringDate, DateOnly.MinValue)
                .With(m => m.hiringType, HIRING_TYPE_EXCLUDED_FROM_MANDATORY_TAXES)
                .With(m => m.computedGrossSalary, 1000.0)
                .CreateMany(1)
                .ToList();

            var result = _mandatoryTaxes.calculateMandatoryTaxes(employees);

            Assert.That(result[0].taxes.employeeCcssSem, Is.EqualTo(employees[0].taxes.employeeCcssSem));
            Assert.That(result[0].taxes.employeeCcssIvm, Is.EqualTo(employees[0].taxes.employeeCcssIvm));
            Assert.That(result[0].taxes.employeeLptBpop, Is.EqualTo(employees[0].taxes.employeeLptBpop));
            Assert.That(result[0].taxes.employeeLptBpop, Is.EqualTo(employees[0].taxes.employeeLptBpop));
            Assert.That(result[0].taxes.employerCcssIvm, Is.EqualTo(employees[0].taxes.employerCcssIvm));
        }

        [Test]
        public void CalculateMandatoryTaxes_CalculatesTaxesCorrectly_WhenHiringTypeNotExcluded()
        {
            var grossSalary = 1000.0;
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.birthDate, DateOnly.MinValue)
                .With(m => m.hiringDate, DateOnly.MinValue)
                .With(m => m.hiringType, string.Empty)
                .With(m => m.computedGrossSalary, grossSalary)
                .CreateMany(1)
                .ToList();

            var result = _mandatoryTaxes.calculateMandatoryTaxes(employees);

            Assert.That(result[0].taxes.employeeCcssSem, Is.EqualTo(grossSalary * 0.055));
            Assert.That(result[0].taxes.employeeCcssIvm, Is.EqualTo(grossSalary * 0.0417));
            Assert.That(result[0].taxes.employeeLptBpop, Is.EqualTo(grossSalary * 0.01));
            Assert.That(result[0].taxes.employerCcssSem, Is.EqualTo(grossSalary * 0.0925));
            Assert.That(result[0].taxes.employerCcssIvm, Is.EqualTo(grossSalary * 0.0542));
            Assert.That(result[0].taxes.employerOthersBpop, Is.EqualTo(grossSalary * 0.0025));
            Assert.That(result[0].taxes.employerOthersFamily, Is.EqualTo(grossSalary * 0.05));
            Assert.That(result[0].taxes.employerOthersImas, Is.EqualTo(grossSalary * 0.005));
            Assert.That(result[0].taxes.employerOthersIna, Is.EqualTo(grossSalary * 0.015));
            Assert.That(result[0].taxes.employerLptBpop, Is.EqualTo(grossSalary * 0.0025));
            Assert.That(result[0].taxes.employerLptFcl, Is.EqualTo(grossSalary * 0.015));
            Assert.That(result[0].taxes.employerLptOpc, Is.EqualTo(grossSalary * 0.02));
            Assert.That(result[0].taxes.employerLptIns, Is.EqualTo(grossSalary * 0.01));
        }

        [Test]
        public void CalculateMandatoryTaxes_CalculatesTaxesCorrectly_WhenMoreThanOneEmployee()
        {
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.birthDate, DateOnly.MinValue)
                .With(m => m.hiringDate, DateOnly.MinValue)
                .With(m => m.hiringType, string.Empty)
                .With(m => m.computedGrossSalary, 1000.0)
                .CreateMany(3)
                .ToList();
            employees[1].hiringType = HIRING_TYPE_EXCLUDED_FROM_MANDATORY_TAXES;

            var result = _mandatoryTaxes.calculateMandatoryTaxes(employees);

            Assert.That(result[0].taxes.employeeCcssSem, Is.Not.EqualTo(0));
            Assert.That(result[1].taxes.employeeCcssSem, Is.EqualTo(employees[1].taxes.employeeCcssSem));
            Assert.That(result[2].taxes.employeeCcssSem, Is.Not.EqualTo(0));
        }

        [Test]
        public void CalculateMandatoryTaxes_CalculatesTaxesCorrectly_WithDecimalPrecision()
        {
            var grossSalary = 1234.56;
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.birthDate, DateOnly.MinValue)
                .With(m => m.hiringDate, DateOnly.MinValue)
                .With(m => m.hiringType, string.Empty)
                .With(m => m.computedGrossSalary, grossSalary)
                .CreateMany(1)
                .ToList();

            var result = _mandatoryTaxes.calculateMandatoryTaxes(employees);

            Assert.That(result[0].taxes.employeeCcssSem, Is.EqualTo(grossSalary * 0.055).Within(0.01));
            Assert.That(result[0].taxes.employerOthersFamily, Is.EqualTo(grossSalary * 0.05).Within(0.01));
        }
    }
}