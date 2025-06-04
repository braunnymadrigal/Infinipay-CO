using Moq;
using NUnit.Framework;
using back_end.Application;
using back_end.Domain;
using back_end.Infraestructure;

namespace Tests
{
  public class PayrollEmployeeTest
  {
    private PayrollEmployee _payrollEmployee;
    private Mock<IPayrollEmployeeRepository> _mockRepo;

    [SetUp]
    public void Setup()
    {
      _mockRepo = new Mock<IPayrollEmployeeRepository>();
      _payrollEmployee = new PayrollEmployee(_mockRepo.Object);
    }

    [Test]
    public void ValidDates_ShouldCallRepository()
    {
      var startDate = new DateOnly(2025, 6, 1);
      var endDate = new DateOnly(2025, 6, 30);

      _mockRepo.Setup(repo =>
          repo.getPayrollEmployees("employer-1", startDate, endDate)
      ).Returns(new List<PayrollEmployeeModel>());

      var result = _payrollEmployee.getPayrollEmployees("employer-1"
        , startDate, endDate);

      Assert.That(result, Is.Empty);
      _mockRepo.Verify(r => r.getPayrollEmployees("employer-1"
        , startDate, endDate), Times.Once);
    }

    [Test]
    public void StartDateEqualsEndDate_ShouldThrowException()
    {
      var date = new DateOnly(2025, 6, 15);
      Assert.Throws<Exception>(() =>
          _payrollEmployee.getPayrollEmployees("employer", date, date)
      );
    }

    [Test]
    public void RangeLongerThan31Days_ShouldThrowException()
    {
      var startDate = new DateOnly(2025, 5, 1);
      var endDate = new DateOnly(2025, 6, 5);
      Assert.Throws<Exception>(() =>
          _payrollEmployee.getPayrollEmployees("employer", startDate
          , endDate)
      );
    }

    [Test]
    public void MinOrMaxDate_ShouldThrowException()
    {
      var validDate = new DateOnly(2025, 6, 1);
      Assert.Throws<Exception>(() =>
          _payrollEmployee.getPayrollEmployees("employer"
          , DateOnly.MinValue, validDate)
      );
      Assert.Throws<Exception>(() =>
          _payrollEmployee.getPayrollEmployees("employer", validDate
          , DateOnly.MaxValue)
      );
    }

    [Test]
    public void DifferentEmployerId_ShouldCallRepositoryWithCorrectId()
    {
      var startDate = new DateOnly(2025, 6, 1);
      var endDate = new DateOnly(2025, 6, 15);
      var employerId = "empresa-123";

      _mockRepo.Setup(r => r.getPayrollEmployees(employerId, startDate
        , endDate))
               .Returns(new List<PayrollEmployeeModel>());

      var result = _payrollEmployee.getPayrollEmployees(employerId, startDate
        , endDate);

      _mockRepo.Verify(r => r.getPayrollEmployees(employerId, startDate
        , endDate), Times.Once);
    }

  }
}
