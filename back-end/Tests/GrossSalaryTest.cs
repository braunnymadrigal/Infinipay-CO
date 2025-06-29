using Moq;
using back_end.Application;
using back_end.Domain;
using AutoFixture;

namespace Tests
{
    class GrossSalaryTest
    {
        private const string EXCEPTION_UNSUPPORTED_PAYMENT_TYPE = "GrossSalary: Improper strategy have been specified.";
        private const string EXCEPTION_SALARY_BELOW_ZERO = "The computed gross salary can not beless than zero.";
        private const string EXCEPTION_SALARY_ABOVE_LIMITS = "The computed gross salary can not exceedthe database limitations";

        private Fixture _fixture;
        private IGrossSalary _grossSalary;
        private Mock<IContextGrossSalaryComputation> _context;
        private IStrategyGrossSalaryComputation _biweekly;
        private IStrategyGrossSalaryComputation _monthly;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _biweekly = new BiweeklyGrossSalaryComputation();
            _monthly = new MonthlyGrossSalaryComputation();
            _context = new Mock<IContextGrossSalaryComputation>();
            _grossSalary = new GrossSalary(_context.Object);
        }

        [Test]
        public void ComputeAllGrossSalaries_ThrowsException_WhenUnsupportedPaymentType()
        {
            var employer = _fixture.Build<PayrollEmployerModel>()
                .With(m => m.startDate, DateOnly.MinValue)
                .With(m => m.endDate, DateOnly.MinValue)
                .With(m => m.latestEndDate, DateOnly.MinValue)
                .With(m => m.paymentType, string.Empty)
                .Create();
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.birthDate, DateOnly.MinValue)
                .With(m => m.hiringDate, DateOnly.MinValue)
                .CreateMany(1)
                .ToList();

            var exception = Assert.Throws<Exception>(() => _grossSalary.computeAllGrossSalaries(employees, employer));
            Assert.That(exception.Message, Is.EqualTo(EXCEPTION_UNSUPPORTED_PAYMENT_TYPE));

            _context.Verify(c => c.setStrategy(It.IsAny<IStrategyGrossSalaryComputation>()), Times.Never);
            _context.Verify(c => c.computeGrossSalary(
                It.IsAny<List<PayrollEmployeeModel>>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()), Times.Never);
        }

        [Test]
        public void ComputeAllGrossSalaries_ThrowsException_WhenSalaryBelowZero()
        {
            var employer = _fixture.Build<PayrollEmployerModel>()
                .With(m => m.startDate, DateOnly.MinValue)
                .With(m => m.endDate, DateOnly.MinValue)
                .With(m => m.latestEndDate, DateOnly.MinValue)
                .With(m => m.paymentType, "quincenal")
                .Create();
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.computedGrossSalary, -1)
                .With(m => m.birthDate, DateOnly.MinValue)
                .With(m => m.hiringDate, DateOnly.MinValue)
                .CreateMany(1)
                .ToList();

            _context.Setup(c => 
                c.setStrategy(It.IsAny<IStrategyGrossSalaryComputation>()))
                .Verifiable();

            _context.Setup(c =>
                c.computeGrossSalary(It.IsAny<List<PayrollEmployeeModel>>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
                .Returns(employees);

            var exception = Assert.Throws<Exception>(() => _grossSalary.computeAllGrossSalaries(employees, employer));
            Assert.That(exception.Message, Is.EqualTo(EXCEPTION_SALARY_BELOW_ZERO));

            _context.Verify(c => c.setStrategy(It.IsAny<IStrategyGrossSalaryComputation>()), Times.Once);
            _context.Verify(c => c.computeGrossSalary(
                It.IsAny<List<PayrollEmployeeModel>>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()), Times.Once);
        }

        [Test]
        public void ComputeAllGrossSalaries_ThrowsException_WhenSalaryAboveLimits()
        {
            var employer = _fixture.Build<PayrollEmployerModel>()
                .With(m => m.startDate, DateOnly.MinValue)
                .With(m => m.endDate, DateOnly.MinValue)
                .With(m => m.latestEndDate, DateOnly.MinValue)
                .With(m => m.paymentType, "quincenal")
                .Create();
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.computedGrossSalary, Double.MaxValue)
                .With(m => m.birthDate, DateOnly.MinValue)
                .With(m => m.hiringDate, DateOnly.MinValue)
                .CreateMany(1)
                .ToList();

            _context.Setup(c =>
                c.setStrategy(It.IsAny<IStrategyGrossSalaryComputation>()))
                .Verifiable();

            _context.Setup(c =>
                c.computeGrossSalary(It.IsAny<List<PayrollEmployeeModel>>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
                .Returns(employees);

            var exception = Assert.Throws<Exception>(() => _grossSalary.computeAllGrossSalaries(employees, employer));
            Assert.That(exception.Message, Is.EqualTo(EXCEPTION_SALARY_ABOVE_LIMITS));

            _context.Verify(c => c.setStrategy(It.IsAny<IStrategyGrossSalaryComputation>()), Times.Once);
            _context.Verify(c => c.computeGrossSalary(
                It.IsAny<List<PayrollEmployeeModel>>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()), Times.Once);
        }

        [Test]
        public void ComputeAllGrossSalaries_ReturnsList_WhenValidValues()
        {
            var employer = _fixture.Build<PayrollEmployerModel>()
                .With(m => m.startDate, DateOnly.MinValue)
                .With(m => m.endDate, DateOnly.MinValue)
                .With(m => m.latestEndDate, DateOnly.MinValue)
                .With(m => m.paymentType, "quincenal")
                .Create();
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.birthDate, DateOnly.MinValue)
                .With(m => m.hiringDate, DateOnly.MinValue)
                .CreateMany(1)
                .ToList();

            _context.Setup(c =>
                c.setStrategy(It.IsAny<IStrategyGrossSalaryComputation>()))
                .Verifiable();

            _context.Setup(c =>
                c.computeGrossSalary(It.IsAny<List<PayrollEmployeeModel>>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
                .Returns(employees);

            var result = _grossSalary.computeAllGrossSalaries(employees, employer);

            Assert.That(result[0].computedGrossSalary, Is.EqualTo(employees[0].computedGrossSalary));

            _context.Verify(c => c.setStrategy(It.IsAny<IStrategyGrossSalaryComputation>()), Times.Once);
            _context.Verify(c => c.computeGrossSalary(
                It.IsAny<List<PayrollEmployeeModel>>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()), Times.Once);
        }

        [Test]
        public void BiweeklyGrossSalaryComputation_ReturnsCorrectValue_WhenHiredBeforeStartDate()
        {
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.rawGrossSalary, 1000.0)
                .With(m => m.hiringDate, new DateOnly(2020, 1, 1))
                .With(m => m.birthDate, DateOnly.MinValue)
                .CreateMany(1)
                .ToList();
            var startDate = new DateOnly(2025, 6, 1);
            var endDate = new DateOnly(2025, 6, 15);
            var expectedResult = 500.0;

            var resultList = _biweekly.computeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void BiweeklyGrossSalaryComputation_ReturnsCorrectValue_WhenHiredOnStartDate()
        {
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.rawGrossSalary, 900.0)
                .With(m => m.hiringDate, new DateOnly(2025, 6, 1))
                .With(m => m.birthDate, DateOnly.MinValue)
                .CreateMany(1)
                .ToList();
            var startDate = new DateOnly(2025, 6, 1);
            var endDate = new DateOnly(2025, 6, 15);
            var expectedResult = 450.0;

            var resultList = _biweekly.computeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            Assert.That(result, Is.EqualTo(expectedResult));
        }


        [Test]
        public void BiweeklyGrossSalaryComputation_ReturnsCorrectValue_WhenHiredAfterStartDate()
        {
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.rawGrossSalary, 900.0)
                .With(m => m.hiringDate, new DateOnly(2025, 6, 10))
                .With(m => m.birthDate, DateOnly.MinValue)
                .CreateMany(1)
                .ToList();
            var startDate = new DateOnly(2025, 6, 1);
            var endDate = new DateOnly(2025, 6, 15);
            var expectedWorkedDays = 6;
            var expectedResult = (450.0 / 15.0) * expectedWorkedDays;

            var resultList = _biweekly.computeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void BiweeklyGrossSalaryComputation_ReturnsCorrectValue_WhenHiredOnLastDay()
        {
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.rawGrossSalary, 600.0)
                .With(m => m.hiringDate, new DateOnly(2025, 6, 15))
                .With(m => m.birthDate, DateOnly.MinValue)
                .CreateMany(1)
                .ToList();
            var startDate = new DateOnly(2025, 6, 1);
            var endDate = new DateOnly(2025, 6, 15);
            var expectedResult = (300.0 / 15.0);

            var resultList = _biweekly.computeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void BiweeklyGrossSalaryComputation_ReturnsCorrectValue_WithDecimalPrecision()
        {
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.rawGrossSalary, 1234.56)
                .With(m => m.hiringDate, new DateOnly(2025, 6, 5))
                .With(m => m.birthDate, DateOnly.MinValue)
                .CreateMany(1)
                .ToList();
            var startDate = new DateOnly(2025, 6, 1);
            var endDate = new DateOnly(2025, 6, 15);
            var expectedWorkedDays = 11;
            var expectedResult = (1234.56 / 2.0 / 15.0) * expectedWorkedDays;

            var resultList = _biweekly.computeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            Assert.That(result, Is.EqualTo(expectedResult).Within(0.01));
        }

        [Test]
        public void MonthlyGrossSalaryComputation_ReturnsCorrectValue_WhenHiredBeforeStartDate()
        {
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.rawGrossSalary, 1500.0)
                .With(m => m.hiringDate, new DateOnly(2020, 1, 1))
                .With(m => m.birthDate, DateOnly.MinValue)
                .CreateMany(1)
                .ToList();
            var startDate = new DateOnly(2025, 6, 1);
            var endDate = new DateOnly(2025, 6, 30);
            var expectedResult = 1500.0;

            var resultList = _monthly.computeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void MonthlyGrossSalaryComputation_ReturnsCorrectValue_WhenHiredOnStartDate()
        {
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.rawGrossSalary, 1800.0)
                .With(m => m.hiringDate, new DateOnly(2025, 6, 1))
                .With(m => m.birthDate, DateOnly.MinValue)
                .CreateMany(1)
                .ToList();
            var startDate = new DateOnly(2025, 6, 1);
            var endDate = new DateOnly(2025, 6, 30);
            var expectedResult = 1800.0;

            var resultList = _monthly.computeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void MonthlyGrossSalaryComputation_ReturnsCorrectValue_WhenHiredAfterStartDate()
        {
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.rawGrossSalary, 2100.0)
                .With(m => m.hiringDate, new DateOnly(2025, 6, 10))
                .With(m => m.birthDate, DateOnly.MinValue)
                .CreateMany(1)
                .ToList();
            var startDate = new DateOnly(2025, 6, 1);
            var endDate = new DateOnly(2025, 6, 30);
            var expectedWorkedDays = 21;
            var expectedResult = (2100.0 / 30.0) * expectedWorkedDays;

            var resultList = _monthly.computeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            Assert.That(result, Is.EqualTo(expectedResult));
        }


        [Test]
        public void MonthlyGrossSalaryComputation_ReturnsCorrectValue_WhenHiredOnLastDay()
        {
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.rawGrossSalary, 3000.0)
                .With(m => m.hiringDate, new DateOnly(2025, 6, 30))
                .With(m => m.birthDate, DateOnly.MinValue)
                .CreateMany(1)
                .ToList();
            var startDate = new DateOnly(2025, 6, 1);
            var endDate = new DateOnly(2025, 6, 30);
            var expectedResult = (3000.0 / 30.0);

            var resultList = _monthly.computeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void MonthlyGrossSalaryComputation_ReturnsCorrectValue_WithDecimalPrecision()
        {
            var employees = _fixture.Build<PayrollEmployeeModel>()
                .With(m => m.rawGrossSalary, 1789.45)
                .With(m => m.hiringDate, new DateOnly(2025, 6, 5))
                .With(m => m.birthDate, DateOnly.MinValue)
                .CreateMany(1)
                .ToList();
            var startDate = new DateOnly(2025, 6, 1);
            var endDate = new DateOnly(2025, 6, 30);
            var expectedWorkedDays = 26;
            var expectedResult = (1789.45 / 30.0) * expectedWorkedDays;

            var resultList = _monthly.computeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            Assert.That(result, Is.EqualTo(expectedResult).Within(0.01));
        }
    }
}
