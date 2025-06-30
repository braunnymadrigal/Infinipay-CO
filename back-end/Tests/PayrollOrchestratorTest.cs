using AutoFixture;
using back_end.Application;
using back_end.Domain;
using Moq;

namespace Tests;

public class PayrollOrchestratorTest
{
  private Mock<IPayrollEmployer> _payrollEmployerMock;
  private Mock<IPayrollEmployee> _payrollEmployeeMock;
  private Mock<IGrossSalary> _grossSalaryMock;
  private Mock<IRentTax> _rentTaxMock;
  private Mock<IMandatoryTaxes> _mandatoryTaxesMock;
  private Mock<IDeduction> _deductionMock;
  private PayrollOrchestrator _sut;
  private Fixture _fixture;

  [SetUp]
  public void Setup()
  {
    _payrollEmployerMock = new Mock<IPayrollEmployer>();
    _payrollEmployeeMock = new Mock<IPayrollEmployee>();
    _grossSalaryMock = new Mock<IGrossSalary>();
    _rentTaxMock = new Mock<IRentTax>();
    _mandatoryTaxesMock = new Mock<IMandatoryTaxes>();
    _deductionMock = new Mock<IDeduction>();

    _sut = new PayrollOrchestrator(
        _payrollEmployerMock.Object,
        _payrollEmployeeMock.Object,
        _grossSalaryMock.Object,
        _rentTaxMock.Object,
        _mandatoryTaxesMock.Object,
        _deductionMock.Object
    );

    _fixture = new Fixture();
  }

  [Test]
  public async Task ComputePayrollAsync_WithValidData_ReturnsCorrectResult()
  {
    var employerId = "empresatesting";
    var start = new DateOnly(2025, 1, 1);
    var end = new DateOnly(2025, 1, 31);

    var employer = new PayrollEmployerModel
    {
      id = "01",
      companyId = employerId,
      paymentType = "mensual",
      startDate = start,
      endDate = end
    };

    var employee = new PayrollEmployeeModel
    {
      id = Guid.NewGuid().ToString() + " extra",
      name = "Carlos González",
      hiringDate = new DateOnly(2023, 2, 15),
      hiringType = "tiempoCompleto",
      rawGrossSalary = 2000,
      computedGrossSalary = 2000,
    };

    var employees = new List<PayrollEmployeeModel> { employee };

    var rentTaxModel = new PayrollEmployeeModel
    {
      taxes = new PayrollTaxModel { employeeRent = 100 }
    };

    var mandatoryTaxModel = new PayrollEmployeeModel
    {
      taxes = new PayrollTaxModel
      {
        employeeCcssSem = 50,
        employeeCcssIvm = 25,
        employeeLptBpop = 10,
        employerCcssSem = 80,
        employerCcssIvm = 60,
        employerOthersBpop = 10,
        employerOthersFamily = 5,
        employerOthersImas = 3,
        employerOthersIna = 2,
        employerLptBpop = 4,
        employerLptFcl = 5,
        employerLptOpc = 6,
        employerLptIns = 7
      }
    };

    var deductions = new List<PayrollEmployeeModel>
        {
            new PayrollEmployeeModel
            {
                deductions = new List<PayrollDeductionModel>
                {
                    new() { name = "Seguro", resultAmount = 70 },
                    new() { name = "Pensión", resultAmount = 30 }
                }
            }
        };

    _payrollEmployerMock
        .Setup(x => x.getPayrollEmployer(employerId, start, end))
        .Returns(employer);

    _payrollEmployeeMock
        .Setup(x => x.getPayrollEmployees(employer))
        .Returns(employees);

    _grossSalaryMock
        .Setup(x => x.computeAllGrossSalaries(employees, employer))
        .Returns(employees);

    _rentTaxMock
        .Setup(x => x.calculateRentTaxes(employees, end))
        .Returns(new List<PayrollEmployeeModel> { rentTaxModel });

    _mandatoryTaxesMock
        .Setup(x => x.calculateMandatoryTaxes(employees))
        .Returns(new List<PayrollEmployeeModel> { mandatoryTaxModel });

    _deductionMock
        .Setup(x => x.calculateDeductions(employees, "mensual"))
        .ReturnsAsync(deductions);

    var result = await _sut.ComputePayrollAsync(employerId, start, end);

    Assert.That(result, Is.Not.Null);
    Assert.That(result.Count, Is.EqualTo(1));
    var item = result.First();
    Assert.That(item.FullName, Is.EqualTo("Carlos González"));
    Assert.That(item.ComputedGrossSalary, Is.EqualTo(2000));
    Assert.That(item.NetSalary, Is.GreaterThan(0));
    Assert.That(item.Deductions.Count, Is.EqualTo(2));
    Assert.That(item.TotalDeductions, Is.EqualTo(100 + 50 + 25 + 10 + 70 + 30));
  }
}