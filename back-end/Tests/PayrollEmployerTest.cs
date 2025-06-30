using Moq;
using NUnit.Framework;
using back_end.Application;
using back_end.Domain;
using back_end.Infraestructure;
using AutoFixture;

namespace Tests
{
    public class PayrollEmployerTest
    {
        private const string EXCEPTION_EMPTY_STRING = "PayrollEmployer: caught invalid string.";
        private const string EXCEPTION_MIN_START_DATE = "PayrollEmployer: start date is equal to DateOnly min value.";
        private const string EXCEPTION_MAX_END_DATE = "PayrollEmployer: end date is equal to DateOnly max value.";
        private const string EXCEPTION_START_DATE_NOT_BEFORE_END_DATE = "PayrollEmployer: start date must be before end date.";
        private const string EXCEPTION_DATES_NOT_IN_SAME_YEAR = "PayrollEmployer: Date range is not in the same year.";
        private const string EXCEPTION_DATES_NOT_IN_SAME_MONTH = "PayrollEmployer: Date range is not in the same month.";
        private const string EXCEPTION_DATES_YEARS_IN_THE_FUTURE = "PayrollEmployer: Date range year is in the future";
        private const string EXCEPTION_START_DATE_NOT_AFTER_LATEST_END_DATE = "PayrollEmployer: Start date should be 1 day after latest end date.";
        private const string EXCEPTION_UNSUPPORTED_PAYMENT_TYPE = "PayrollEmployer: Payment type not supported.";
        private const string EXCEPTION_BIWEEKLY_UNSUPPORTED_START_DATE = "PayrollEmployer: Biweekly start date not supported.";
        private const string EXCEPTION_MONTHLY_UNSUPPORTED_START_DATE = "PayrollEmployer: Monthly start date not supported.";
        private const string EXCEPTION_FIRST_HALF_BIWEEKLY_UNSUPPORTED_END_DATE = "PayrollEmployer: first half of month must end the 15.";
        private const string EXCEPTION_END_DATE_IS_NOT_LAST_DAY_OF_MONTH = "PayrollEmployer: end date must be the last day of the month.";

        private PayrollEmployer _payrollEmployer;
        private Mock<IPayrollEmployerRepository> _payrollEmployerRepository;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _payrollEmployerRepository = new Mock<IPayrollEmployerRepository>();
            _payrollEmployer = new PayrollEmployer(_payrollEmployerRepository.Object);
            _fixture = new Fixture();
        }

        [Test]
        public void GetPayrollEmployer_ThrowsException_WhenPaymentTypeIsEmpty()
        {
            var returnedModel = _fixture.Build<PayrollEmployerModel>()
                .With(m => m.startDate, DateOnly.MinValue)
                .With(m => m.endDate, DateOnly.MinValue)
                .With(m => m.latestEndDate, DateOnly.MinValue)
                .With(m => m.paymentType, string.Empty)
                .Create();

            _payrollEmployerRepository.Setup(r =>
                r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()))
                .Returns(returnedModel);
            _payrollEmployerRepository.Setup(r => r.getDateOfLatestPayroll(It.IsAny<string>()))
                .Returns(DateOnly.MinValue);

            var exception = Assert.Throws<Exception>(() => _payrollEmployer.getPayrollEmployer(string.Empty, 
                DateOnly.MinValue, DateOnly.MaxValue));

            Assert.That(exception.Message, Is.EqualTo(EXCEPTION_EMPTY_STRING));

            _payrollEmployerRepository.Verify(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()), Times.Once);
            _payrollEmployerRepository.Verify(r => r.getDateOfLatestPayroll(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void GetPayrollEmployer_ThrowsException_WhenStartDateIsMinValue()
        {
            var paymentType = "quincenal";
            var endDate = new DateOnly(2024, 1, 16);

            var returnedModel = _fixture.Build<PayrollEmployerModel>()
                .With(m => m.startDate, DateOnly.MinValue)
                .With(m => m.endDate, endDate)
                .With(m => m.latestEndDate, DateOnly.MinValue)
                .With(m => m.paymentType, paymentType)
                .Create();

            _payrollEmployerRepository.Setup(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()))
                .Returns(returnedModel);
            _payrollEmployerRepository.Setup(r => r.getDateOfLatestPayroll(It.IsAny<string>()))
                .Returns(DateOnly.MinValue);

            var exception = Assert.Throws<Exception>(() =>
                _payrollEmployer.getPayrollEmployer(string.Empty, DateOnly.MinValue, DateOnly.MinValue));

            Assert.That(exception.Message, Is.EqualTo(EXCEPTION_MIN_START_DATE));

            _payrollEmployerRepository.Verify(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()), Times.Once);
            _payrollEmployerRepository.Verify(r => r.getDateOfLatestPayroll(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GetPayrollEmployer_ThrowsException_WhenEndDateIsMaxValue()
        {
            var paymentType = "quincenal";
            var startDate = new DateOnly(2024, 1, 1);

            var returnedModel = _fixture.Build<PayrollEmployerModel>()
                .With(m => m.startDate, startDate)
                .With(m => m.endDate, DateOnly.MaxValue)
                .With(m => m.latestEndDate, DateOnly.MinValue)
                .With(m => m.paymentType, paymentType)
                .Create();

            _payrollEmployerRepository.Setup(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()))
                .Returns(returnedModel);
            _payrollEmployerRepository.Setup(r => r.getDateOfLatestPayroll(It.IsAny<string>()))
                .Returns(DateOnly.MinValue);

            var exception = Assert.Throws<Exception>(() =>
                _payrollEmployer.getPayrollEmployer(string.Empty, DateOnly.MinValue, DateOnly.MinValue));

            Assert.That(exception.Message, Is.EqualTo(EXCEPTION_MAX_END_DATE));

            _payrollEmployerRepository.Verify(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()), Times.Once);
            _payrollEmployerRepository.Verify(r => r.getDateOfLatestPayroll(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GetPayrollEmployer_ThrowsException_WhenStartDateNotBeforeEndDate()
        {
            var paymentType = "quincenal";
            var startDate = new DateOnly(2024, 1, 16);
            var endDate = new DateOnly(2024, 1, 16);

            var returnedModel = _fixture.Build<PayrollEmployerModel>()
                .With(m => m.startDate, startDate)
                .With(m => m.endDate, endDate)
                .With(m => m.latestEndDate, DateOnly.MinValue)
                .With(m => m.paymentType, paymentType)
                .Create();

            _payrollEmployerRepository.Setup(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()))
                .Returns(returnedModel);
            _payrollEmployerRepository.Setup(r => r.getDateOfLatestPayroll(It.IsAny<string>()))
                .Returns(DateOnly.MinValue);

            var ex = Assert.Throws<Exception>(() =>
                _payrollEmployer.getPayrollEmployer(string.Empty, DateOnly.MinValue, DateOnly.MinValue));

            Assert.That(ex.Message, Is.EqualTo(EXCEPTION_START_DATE_NOT_BEFORE_END_DATE));

            _payrollEmployerRepository.Verify(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()), Times.Once);
            _payrollEmployerRepository.Verify(r => r.getDateOfLatestPayroll(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GetPayrollEmployer_ThrowsException_WhenDatesNotInSameYear()
        {
            var paymentType = "quincenal";
            var startDate = new DateOnly(2024, 1, 1);
            var endDate = new DateOnly(2025, 1, 31);

            var returnedModel = _fixture.Build<PayrollEmployerModel>()
                .With(m => m.startDate, startDate)
                .With(m => m.endDate, endDate)
                .With(m => m.latestEndDate, DateOnly.MinValue)
                .With(m => m.paymentType, paymentType)
                .Create();

            _payrollEmployerRepository.Setup(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()))
                .Returns(returnedModel);
            _payrollEmployerRepository.Setup(r => r.getDateOfLatestPayroll(It.IsAny<string>()))
                .Returns(DateOnly.MinValue);

            var ex = Assert.Throws<Exception>(() =>
                _payrollEmployer.getPayrollEmployer(string.Empty, DateOnly.MinValue, DateOnly.MinValue));

            Assert.That(ex.Message, Is.EqualTo(EXCEPTION_DATES_NOT_IN_SAME_YEAR));

            _payrollEmployerRepository.Verify(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()), Times.Once);
            _payrollEmployerRepository.Verify(r => r.getDateOfLatestPayroll(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GetPayrollEmployer_ThrowsException_WhenDatesNotInSameMonth()
        {
            var paymentType = "quincenal";
            var startDate = new DateOnly(2024, 1, 1);
            var endDate = new DateOnly(2024, 2, 28);

            var returnedModel = _fixture.Build<PayrollEmployerModel>()
                .With(m => m.startDate, startDate)
                .With(m => m.endDate, endDate)
                .With(m => m.latestEndDate, DateOnly.MinValue)
                .With(m => m.paymentType, paymentType)
                .Create();

            _payrollEmployerRepository.Setup(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()))
                .Returns(returnedModel);
            _payrollEmployerRepository.Setup(r => r.getDateOfLatestPayroll(It.IsAny<string>()))
                .Returns(DateOnly.MinValue);

            var ex = Assert.Throws<Exception>(() =>
                _payrollEmployer.getPayrollEmployer(string.Empty, DateOnly.MinValue, DateOnly.MinValue));

            Assert.That(ex.Message, Is.EqualTo(EXCEPTION_DATES_NOT_IN_SAME_MONTH));

            _payrollEmployerRepository.Verify(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()), Times.Once);
            _payrollEmployerRepository.Verify(r => r.getDateOfLatestPayroll(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GetPayrollEmployer_ThrowsException_WhenDatesAreYearsInTheFuture()
        {
            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
            var paymentType = "quincenal";
            var startDate = new DateOnly(currentDate.Year + 1, 1, 1);
            var endDate = new DateOnly(currentDate.Year + 1, 1, 31);

            var returnedModel = _fixture.Build<PayrollEmployerModel>()
                .With(m => m.startDate, startDate)
                .With(m => m.endDate, endDate)
                .With(m => m.latestEndDate, DateOnly.MinValue)
                .With(m => m.paymentType, paymentType)
                .Create();

            _payrollEmployerRepository.Setup(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()))
                .Returns(returnedModel);
            _payrollEmployerRepository.Setup(r => r.getDateOfLatestPayroll(It.IsAny<string>()))
                .Returns(DateOnly.MinValue);

            var ex = Assert.Throws<Exception>(() =>
                _payrollEmployer.getPayrollEmployer(string.Empty, DateOnly.MinValue, DateOnly.MinValue));

            Assert.That(ex.Message, Is.EqualTo(EXCEPTION_DATES_YEARS_IN_THE_FUTURE));

            _payrollEmployerRepository.Verify(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()), Times.Once);
            _payrollEmployerRepository.Verify(r => r.getDateOfLatestPayroll(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GetPayrollEmployer_ThrowsException_WhenStartDateNotAfterLatestEndDate()
        {
            var paymentType = "quincenal";
            var startDate = new DateOnly(2025, 2, 1);
            var endDate = new DateOnly(2025, 2, 28);
            var latestEndDate = new DateOnly(2025, 1, 30);

            var returnedModel = _fixture.Build<PayrollEmployerModel>()
                .With(m => m.startDate, startDate)
                .With(m => m.endDate, endDate)
                .With(m => m.latestEndDate, DateOnly.MinValue)
                .With(m => m.paymentType, paymentType)
                .Create();

            _payrollEmployerRepository.Setup(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()))
                .Returns(returnedModel);
            _payrollEmployerRepository.Setup(r => r.getDateOfLatestPayroll(It.IsAny<string>()))
                .Returns(latestEndDate);

            var ex = Assert.Throws<Exception>(() =>
                _payrollEmployer.getPayrollEmployer(string.Empty, DateOnly.MinValue, DateOnly.MinValue));

            Assert.That(ex.Message, Is.EqualTo(EXCEPTION_START_DATE_NOT_AFTER_LATEST_END_DATE));

            _payrollEmployerRepository.Verify(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()), Times.Once);
            _payrollEmployerRepository.Verify(r => r.getDateOfLatestPayroll(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GetPayrollEmployer_ThrowsException_WhenPaymentTypeIsUnsupported()
        {
            var paymentType = "semanal";
            var startDate = new DateOnly(2025, 1, 1);
            var endDate = new DateOnly(2025, 1, 31);

            var returnedModel = _fixture.Build<PayrollEmployerModel>()
                .With(m => m.startDate, startDate)
                .With(m => m.endDate, endDate)
                .With(m => m.latestEndDate, DateOnly.MinValue)
                .With(m => m.paymentType, paymentType)
                .Create();

            _payrollEmployerRepository.Setup(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()))
                .Returns(returnedModel);
            _payrollEmployerRepository.Setup(r => r.getDateOfLatestPayroll(It.IsAny<string>()))
                .Returns(DateOnly.MinValue);

            var ex = Assert.Throws<Exception>(() =>
                _payrollEmployer.getPayrollEmployer(string.Empty, DateOnly.MinValue, DateOnly.MinValue));

            Assert.That(ex.Message, Is.EqualTo(EXCEPTION_UNSUPPORTED_PAYMENT_TYPE));

            _payrollEmployerRepository.Verify(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()), Times.Once);
            _payrollEmployerRepository.Verify(r => r.getDateOfLatestPayroll(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GetPayrollEmployer_ThrowsException_WhenStartDateIsUnsupportedByBiweekly()
        {
            var paymentType = "quincenal";
            var startDate = new DateOnly(2025, 1, 15);
            var endDate = new DateOnly(2025, 1, 31);

            var returnedModel = _fixture.Build<PayrollEmployerModel>()
                .With(m => m.startDate, startDate)
                .With(m => m.endDate, endDate)
                .With(m => m.latestEndDate, DateOnly.MinValue)
                .With(m => m.paymentType, paymentType)
                .Create();

            _payrollEmployerRepository.Setup(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()))
                .Returns(returnedModel);
            _payrollEmployerRepository.Setup(r => r.getDateOfLatestPayroll(It.IsAny<string>()))
                .Returns(DateOnly.MinValue);

            var ex = Assert.Throws<Exception>(() =>
                _payrollEmployer.getPayrollEmployer(string.Empty, DateOnly.MinValue, DateOnly.MinValue));

            Assert.That(ex.Message, Is.EqualTo(EXCEPTION_BIWEEKLY_UNSUPPORTED_START_DATE));

            _payrollEmployerRepository.Verify(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()), Times.Once);
            _payrollEmployerRepository.Verify(r => r.getDateOfLatestPayroll(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GetPayrollEmployer_ThrowsException_WhenStartDateIsUnsupportedByMonthly()
        {
            var paymentType = "mensual";
            var startDate = new DateOnly(2025, 1, 16);
            var endDate = new DateOnly(2025, 1, 31);

            var returnedModel = _fixture.Build<PayrollEmployerModel>()
                .With(m => m.startDate, startDate)
                .With(m => m.endDate, endDate)
                .With(m => m.latestEndDate, DateOnly.MinValue)
                .With(m => m.paymentType, paymentType)
                .Create();

            _payrollEmployerRepository.Setup(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()))
                .Returns(returnedModel);
            _payrollEmployerRepository.Setup(r => r.getDateOfLatestPayroll(It.IsAny<string>()))
                .Returns(DateOnly.MinValue);

            var ex = Assert.Throws<Exception>(() =>
                _payrollEmployer.getPayrollEmployer(string.Empty, DateOnly.MinValue, DateOnly.MinValue));

            Assert.That(ex.Message, Is.EqualTo(EXCEPTION_MONTHLY_UNSUPPORTED_START_DATE));

            _payrollEmployerRepository.Verify(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()), Times.Once);
            _payrollEmployerRepository.Verify(r => r.getDateOfLatestPayroll(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GetPayrollEmployer_ThrowsException_WhenFirstHalfEndDateIsUnsupportedByBiweekly()
        {
            var paymentType = "quincenal";
            var startDate = new DateOnly(2025, 1, 1);
            var endDate = new DateOnly(2025, 1, 16);

            var returnedModel = _fixture.Build<PayrollEmployerModel>()
                .With(m => m.startDate, startDate)
                .With(m => m.endDate, endDate)
                .With(m => m.latestEndDate, DateOnly.MinValue)
                .With(m => m.paymentType, paymentType)
                .Create();

            _payrollEmployerRepository.Setup(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()))
                .Returns(returnedModel);
            _payrollEmployerRepository.Setup(r => r.getDateOfLatestPayroll(It.IsAny<string>()))
                .Returns(DateOnly.MinValue);

            var ex = Assert.Throws<Exception>(() =>
                _payrollEmployer.getPayrollEmployer(string.Empty, DateOnly.MinValue, DateOnly.MinValue));

            Assert.That(ex.Message, Is.EqualTo(EXCEPTION_FIRST_HALF_BIWEEKLY_UNSUPPORTED_END_DATE));

            _payrollEmployerRepository.Verify(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()), Times.Once);
            _payrollEmployerRepository.Verify(r => r.getDateOfLatestPayroll(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GetPayrollEmployer_ThrowsException_WhenEndDateIsNotTheLastDayOfTheMonth()
        {
            var paymentType = "quincenal";
            var startDate = new DateOnly(2025, 1, 16);
            var endDate = new DateOnly(2025, 1, 30);

            var returnedModel = _fixture.Build<PayrollEmployerModel>()
                .With(m => m.startDate, startDate)
                .With(m => m.endDate, endDate)
                .With(m => m.latestEndDate, DateOnly.MinValue)
                .With(m => m.paymentType, paymentType)
                .Create();

            _payrollEmployerRepository.Setup(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()))
                .Returns(returnedModel);
            _payrollEmployerRepository.Setup(r => r.getDateOfLatestPayroll(It.IsAny<string>()))
                .Returns(DateOnly.MinValue);

            var ex = Assert.Throws<Exception>(() =>
                _payrollEmployer.getPayrollEmployer(string.Empty, DateOnly.MinValue, DateOnly.MinValue));

            Assert.That(ex.Message, Is.EqualTo(EXCEPTION_END_DATE_IS_NOT_LAST_DAY_OF_MONTH));

            _payrollEmployerRepository.Verify(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()), Times.Once);
            _payrollEmployerRepository.Verify(r => r.getDateOfLatestPayroll(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GetPayrollEmployer_ReturnsModel_WhenValidMonthlyDates()
        {
            var paymentType = "mensual";
            var startDate = new DateOnly(2025, 1, 1);
            var endDate = new DateOnly(2025, 1, 31);

            var returnedModel = _fixture.Build<PayrollEmployerModel>()
                .With(m => m.startDate, startDate)
                .With(m => m.endDate, endDate)
                .With(m => m.latestEndDate, DateOnly.MinValue)
                .With(m => m.paymentType, paymentType)
                .Create();

            _payrollEmployerRepository.Setup(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()))
                .Returns(returnedModel);
            _payrollEmployerRepository.Setup(r => r.getDateOfLatestPayroll(It.IsAny<string>()))
                .Returns(DateOnly.MinValue);

            var result = _payrollEmployer.getPayrollEmployer(string.Empty, DateOnly.MinValue, DateOnly.MinValue);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.paymentType, Is.EqualTo(returnedModel.paymentType));
            Assert.That(result.startDate, Is.EqualTo(returnedModel.startDate));

            _payrollEmployerRepository.Verify(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()), Times.Once);
            _payrollEmployerRepository.Verify(r => r.getDateOfLatestPayroll(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GetPayrollEmployer_ReturnsModel_WhenValidBiweeklyDates()
        {
            var paymentType = "quincenal";
            var startDate = new DateOnly(2025, 1, 1);
            var endDate = new DateOnly(2025, 1, 15);

            var returnedModel = _fixture.Build<PayrollEmployerModel>()
                .With(m => m.startDate, startDate)
                .With(m => m.endDate, endDate)
                .With(m => m.latestEndDate, DateOnly.MinValue)
                .With(m => m.paymentType, paymentType)
                .Create();

            _payrollEmployerRepository.Setup(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()))
                .Returns(returnedModel);
            _payrollEmployerRepository.Setup(r => r.getDateOfLatestPayroll(It.IsAny<string>()))
                .Returns(DateOnly.MinValue);

            var result = _payrollEmployer.getPayrollEmployer(string.Empty, DateOnly.MinValue, DateOnly.MinValue);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.paymentType, Is.EqualTo(returnedModel.paymentType));
            Assert.That(result.startDate, Is.EqualTo(returnedModel.startDate));

            _payrollEmployerRepository.Verify(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()), Times.Once);
            _payrollEmployerRepository.Verify(r => r.getDateOfLatestPayroll(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GetPaymentType_ThrowsException_WhenPaymentTypeIsEmpty()
        {
            var paymentType = string.Empty;

            var returnedModel = _fixture.Build<PayrollEmployerModel>()
                .With(m => m.startDate, DateOnly.MinValue)
                .With(m => m.endDate, DateOnly.MinValue)
                .With(m => m.latestEndDate, DateOnly.MinValue)
                .With(m => m.paymentType, paymentType)
                .Create();

            _payrollEmployerRepository.Setup(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()))
                .Returns(returnedModel);

            var exception = Assert.Throws<Exception>(() => _payrollEmployer.getPaymentType(string.Empty));

            Assert.That(exception.Message, Is.EqualTo(EXCEPTION_EMPTY_STRING));

            _payrollEmployerRepository.Verify(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()), Times.Once);
        }

        [Test]
        public void GetPaymentType_ReturnsString_WhenPaymentTypeIsNotEmpty()
        {
            var paymentType = "a";

            var returnedModel = _fixture.Build<PayrollEmployerModel>()
                .With(m => m.startDate, DateOnly.MinValue)
                .With(m => m.endDate, DateOnly.MinValue)
                .With(m => m.latestEndDate, DateOnly.MinValue)
                .With(m => m.paymentType, paymentType)
                .Create();

            _payrollEmployerRepository.Setup(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()))
                .Returns(returnedModel);

            var result = _payrollEmployer.getPaymentType(string.Empty);

            Assert.That(result, Is.EqualTo(paymentType));

            _payrollEmployerRepository.Verify(r => r.getPayrollEmployer(It.IsAny<PayrollEmployerModel>()), Times.Once);
        }
    }
}
