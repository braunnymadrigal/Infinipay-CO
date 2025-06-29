using AutoFixture;
using back_end.Application;
using back_end.Domain;

namespace Tests
{
    class DeductionTest
    {
        private const string GEEMS_API_URL = "https://asociacion-geems-c3dfavfsapguhxbp.southcentralus-01.azurewebsites.net/api/public/calculator/calculate";
        private const string FORMULA_TYPE_FIXED_AMOUNT = "montoFijo";
        private const string FORMULA_TYPE_PERCENTAGE = "porcentaje";
        private const string FORMULA_TYPE_API = "api";
        private const string PAYMENT_TYPE_BIWEEKLY = "quincenal";
        private const string PAYMENT_TYPE_MONTHLY = "mensual";

        private const string EXCEPTION_UNSUPPORTED_FORMULA_TYPE = "Deduction: type of deduction is not supported.";
        private const string EXCEPTION_UNSUPPORTED_PAYMENT_TYPE = "Deduction: Payment type not supported.";
        private const string EXCEPTION_STRING_IS_NOT_A_DOUBLE = "The string does not represent a positive proper double number.";
        private const string EXCEPTION_UNSUPPORTED_API_URL = "Unknown API can not be used.";

        private Fixture _fixture;
        private IDeduction _deduction;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _deduction = new Deduction();
        }

        [Test]
        public void CalculateDeductions_ThrowsException_WhenUnsupportedFormulaType()
        {
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.birthDate, DateOnly.MinValue)
                .With(m => m.hiringDate, DateOnly.MinValue)
                .With(m => m.deductions, new List<PayrollDeductionModel>
                {
                    _fixture.Build<PayrollDeductionModel>()
                        .With(d => d.formulaType, "invalid")
                        .Create()
                })
                .CreateMany(1)
                .ToList();

            var exception = Assert.ThrowsAsync<Exception>(async () =>
            {
                await _deduction.calculateDeductions(employees, string.Empty);
            });
            Assert.That(exception.Message, Is.EqualTo(EXCEPTION_UNSUPPORTED_FORMULA_TYPE));
        }

        [Test]
        public void CalculateDeductions_ThrowsException_WhenUnsupportedPaymentType()
        {
            var fixedAmount = 2500.50;
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.birthDate, DateOnly.MinValue)
                .With(m => m.hiringDate, DateOnly.MinValue)
                .With(m => m.deductions, new List<PayrollDeductionModel>
                {
                    _fixture.Build<PayrollDeductionModel>()
                        .With(d => d.formulaType, FORMULA_TYPE_FIXED_AMOUNT)
                        .With(d => d.param1Value, fixedAmount.ToString())
                        .Create()
                })
                .CreateMany(1)
                .ToList();

            var exception = Assert.ThrowsAsync<Exception>(async () =>
            {
                await _deduction.calculateDeductions(employees, string.Empty);
            });
            Assert.That(exception.Message, Is.EqualTo(EXCEPTION_UNSUPPORTED_PAYMENT_TYPE));
        }

        [Test]
        public async Task CalculateDeductions_ReturnsHalvedAmount_WhenValidFixedAmountBiweekly()
        {
            var fixedAmount = 2500.50;
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.birthDate, DateOnly.MinValue)
                .With(m => m.hiringDate, DateOnly.MinValue)
                .With(m => m.deductions, new List<PayrollDeductionModel>
                {
                    _fixture.Build<PayrollDeductionModel>()
                        .With(d => d.formulaType, FORMULA_TYPE_FIXED_AMOUNT)
                        .With(d => d.param1Value, fixedAmount.ToString())
                        .Create()
                })
                .CreateMany(1)
                .ToList();

            var result = await _deduction.calculateDeductions(employees, PAYMENT_TYPE_BIWEEKLY);

            Assert.That(result[0].deductions[0].resultAmount, Is.EqualTo(fixedAmount / 2));
        }

        [Test]
        public async Task CalculateDeductions_ReturnsTotalAmount_WhenValidFixedAmountMonthly()
        {
            var fixedAmount = 2500.50;
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.birthDate, DateOnly.MinValue)
                .With(m => m.hiringDate, DateOnly.MinValue)
                .With(m => m.deductions, new List<PayrollDeductionModel>
                {
                    _fixture.Build<PayrollDeductionModel>()
                        .With(d => d.formulaType, FORMULA_TYPE_FIXED_AMOUNT)
                        .With(d => d.param1Value, fixedAmount.ToString())
                        .Create()
                })
                .CreateMany(1)
                .ToList();

            var result = await _deduction.calculateDeductions(employees, PAYMENT_TYPE_MONTHLY);

            Assert.That(result[0].deductions[0].resultAmount, Is.EqualTo(fixedAmount));
        }

        [Test]
        public void CalculateDeductions_ThrowsException_WhenNegativeFixedAmount()
        {
            var fixedAmount = -2500.50;
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.birthDate, DateOnly.MinValue)
                .With(m => m.hiringDate, DateOnly.MinValue)
                .With(m => m.deductions, new List<PayrollDeductionModel>
                {
                    _fixture.Build<PayrollDeductionModel>()
                        .With(d => d.formulaType, FORMULA_TYPE_FIXED_AMOUNT)
                        .With(d => d.param1Value, fixedAmount.ToString())
                        .Create()
                })
                .CreateMany(1)
                .ToList();

            var exception = Assert.ThrowsAsync<Exception>(async () =>
            {
                await _deduction.calculateDeductions(employees, PAYMENT_TYPE_MONTHLY);
            });
            Assert.That(exception.Message, Is.EqualTo(EXCEPTION_STRING_IS_NOT_A_DOUBLE));
        }

        [Test]
        public void CalculateDeductions_ThrowsException_WhenNotNumberFixedAmount()
        {
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.birthDate, DateOnly.MinValue)
                .With(m => m.hiringDate, DateOnly.MinValue)
                .With(m => m.deductions, new List<PayrollDeductionModel>
                {
                    _fixture.Build<PayrollDeductionModel>()
                        .With(d => d.formulaType, FORMULA_TYPE_FIXED_AMOUNT)
                        .With(d => d.param1Value, string.Empty)
                        .Create()
                })
                .CreateMany(1)
                .ToList();

            var exception = Assert.ThrowsAsync<Exception>(async () =>
            {
                await _deduction.calculateDeductions(employees, PAYMENT_TYPE_MONTHLY);
            });
            Assert.That(exception.Message, Is.EqualTo(EXCEPTION_STRING_IS_NOT_A_DOUBLE));
        }

        [Test]
        public async Task CalculateDeducions_ReturnsAmount_WhenValidPercentage()
        {
            var salary = 1000.0;
            var percentage = 10;
            var expectedResult = 100.0;
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.birthDate, DateOnly.MinValue)
                .With(m => m.hiringDate, DateOnly.MinValue)
                .With(m => m.computedGrossSalary, salary)
                .With(m => m.deductions, new List<PayrollDeductionModel>
                {
                    _fixture.Build<PayrollDeductionModel>()
                        .With(d => d.formulaType, FORMULA_TYPE_PERCENTAGE)
                        .With(d => d.param1Value, percentage.ToString())
                        .Create()
                })
                .CreateMany(1)
                .ToList();

            var result = await _deduction.calculateDeductions(employees, PAYMENT_TYPE_BIWEEKLY);

            Assert.That(result[0].deductions[0].resultAmount, Is.EqualTo(expectedResult));
        }

        [Test]
        public void CalculateDeducions_ThrowsException_WhenNegativePercentage()
        {
            var salary = 1000.0;
            var percentage = -10;
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.birthDate, DateOnly.MinValue)
                .With(m => m.hiringDate, DateOnly.MinValue)
                .With(m => m.computedGrossSalary, salary)
                .With(m => m.deductions, new List<PayrollDeductionModel>
                {
                    _fixture.Build<PayrollDeductionModel>()
                        .With(d => d.formulaType, FORMULA_TYPE_PERCENTAGE)
                        .With(d => d.param1Value, percentage.ToString())
                        .Create()
                })
                .CreateMany(1)
                .ToList();

            var exception = Assert.ThrowsAsync<Exception>(async () =>
            {
                await _deduction.calculateDeductions(employees, PAYMENT_TYPE_BIWEEKLY);
            });
            Assert.That(exception.Message, Is.EqualTo(EXCEPTION_STRING_IS_NOT_A_DOUBLE));
        }

        [Test]
        public void CalculateDeducions_ThrowsException_WhenNotNumberPercentage()
        {
            var salary = 1000.0;
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.birthDate, DateOnly.MinValue)
                .With(m => m.hiringDate, DateOnly.MinValue)
                .With(m => m.computedGrossSalary, salary)
                .With(m => m.deductions, new List<PayrollDeductionModel>
                {
                    _fixture.Build<PayrollDeductionModel>()
                        .With(d => d.formulaType, FORMULA_TYPE_PERCENTAGE)
                        .With(d => d.param1Value, string.Empty)
                        .Create()
                })
                .CreateMany(1)
                .ToList();

            var exception = Assert.ThrowsAsync<Exception>(async () =>
            {
                await _deduction.calculateDeductions(employees, PAYMENT_TYPE_BIWEEKLY);
            });
            Assert.That(exception.Message, Is.EqualTo(EXCEPTION_STRING_IS_NOT_A_DOUBLE));
        }

        [Test]
        public void CalculateDeductions_ThrowsException_WhenUnsupportedApiUrl()
        {
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.birthDate, DateOnly.MinValue)
                .With(m => m.hiringDate, DateOnly.MinValue)
                .With(m => m.deductions, new List<PayrollDeductionModel>
                {
                    _fixture.Build<PayrollDeductionModel>()
                        .With(d => d.formulaType, FORMULA_TYPE_API)
                        .With(d => d.apiUrl, string.Empty)
                        .Create()
                })
                .CreateMany(1)
                .ToList();

            var exception = Assert.ThrowsAsync<Exception>(async () =>
            {
                await _deduction.calculateDeductions(employees, PAYMENT_TYPE_MONTHLY);
            });
            Assert.That(exception.Message, Is.EqualTo(EXCEPTION_UNSUPPORTED_API_URL));
        }

        [Test]
        public async Task CalculateDeductions_ReturnsAmount_WhenSupportedApiUrl()
        {
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.birthDate, DateOnly.MinValue)
                .With(m => m.hiringDate, DateOnly.MinValue)
                .With(m => m.computedGrossSalary, 30000.25)
                .With(m => m.companyAssociation, "a")
                .With(m => m.deductions, new List<PayrollDeductionModel>
                {
                    _fixture.Build<PayrollDeductionModel>()
                        .With(d => d.formulaType, FORMULA_TYPE_API)
                        .With(d => d.apiUrl, GEEMS_API_URL)
                        .With(d => d.header1Key, "API-KEY")
                        .With(d => d.header1Value, "Tralalerotralala")
                        .With(d => d.param1Key, "associationName")
                        .With(d => d.param2Key, "employeeSalary")
                        .With(d => d.resultAmount, Double.MinValue)
                        .Create()
                })
                .CreateMany(1)
                .ToList();

            var result = await _deduction.calculateDeductions(employees, PAYMENT_TYPE_BIWEEKLY);

            Assert.That(result[0].deductions[0].resultAmount, Is.GreaterThan(Double.MinValue));
        }
    }
}
