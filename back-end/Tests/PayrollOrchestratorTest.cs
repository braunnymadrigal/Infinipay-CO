using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using back_end.Application;
using back_end.Domain;
using Moq;
using NUnit.Framework;

namespace Tests
{
  public class PayrollOrchestratorTest
  {
    [Test]
    public async Task ComputePayrollAsync_ShouldComputeCorrectNetSalary()
    {
      var mockPayrollEmployee = new Mock<IPayrollEmployee>();
      var mockGrossSalary = new Mock<IGrossSalary>();
      var mockRentTax = new Mock<IRentTax>();
      var mockCCSS = new Mock<ITaxCCSS>();
      var mockDeduction = new Mock<IDeduction>();

      var employeeId = Guid.NewGuid().ToString() + " abc";

      var employee = new PayrollEmployeeModel
      {
        gender = "",
        birthDate = DateOnly.MinValue,
        rentTax = 0,
        rawGrossSalary = 0,
        ccssEmployeeDeduction = 0,
        ccssEmployerDeduction = 0,
        hoursDate = DateOnly.MinValue,
        hoursNumber = 0,
        companyAssociation = "",
        deductions = new List<PayrollDeductionModel>(),
        previousComputedGrossSalaries =
          new List<PayrollPreviousComputedGrossSalary>(),

        id = employeeId,
        fullName = "Juan Pérez",
        hiringDate = new DateOnly(2020, 1, 1),
        hiringType = "planilla",
        computedGrossSalary = 1000
      };

      mockPayrollEmployee
        .Setup(p => p.getPayrollEmployees("EMP123", It.IsAny<DateOnly>()
          , It.IsAny<DateOnly>()))
        .Returns(new List<PayrollEmployeeModel> { employee });

      mockGrossSalary
        .Setup(g =>
          g.computeAllGrossSalaries(It.IsAny<List<PayrollEmployeeModel>>()
          , It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
        .Returns(new List<PayrollEmployeeModel> { employee });

      mockRentTax
        .Setup(r => r.calculateRentTaxes(It.IsAny<List<PayrollEmployeeModel>>()
          , It.IsAny<DateOnly>()))
        .Returns(new List<PayrollEmployeeModel> {
          new PayrollEmployeeModel {
            id = employeeId,
            rentTax = 100,
            computedGrossSalary = 1000,

            gender = "",
            birthDate = DateOnly.MinValue,
            rawGrossSalary = 0,
            ccssEmployeeDeduction = 0,
            ccssEmployerDeduction = 0,
            hoursDate = DateOnly.MinValue,
            hoursNumber = 0,
            companyAssociation = "",
            deductions = new List<PayrollDeductionModel>(),
            previousComputedGrossSalaries =
              new List<PayrollPreviousComputedGrossSalary>(),
            fullName = "",
            hiringDate = new DateOnly(2020, 1, 1),
            hiringType = "tiempoCompleto"
          }
        });

      mockCCSS
        .Setup(c => c.computeTaxesCCSS(It.IsAny<List<PayrollEmployeeModel>>()
          , It.IsAny<DateOnly>()))
        .Returns(new List<PayrollEmployeeModel> {
          new PayrollEmployeeModel {
            id = employeeId,
            ccssEmployeeDeduction = 50,
            computedGrossSalary = 1000,
            gender = "",

            birthDate = DateOnly.MinValue,
            rentTax = 0,
            rawGrossSalary = 0,
            ccssEmployerDeduction = 0,
            hoursDate = DateOnly.MinValue,
            hoursNumber = 0,
            companyAssociation = "",
            deductions = new List<PayrollDeductionModel>(),
            previousComputedGrossSalaries =
              new List<PayrollPreviousComputedGrossSalary>(),
            fullName = "",
            hiringDate = new DateOnly(2020, 1, 1),
            hiringType = "tiempoCompleto",
          }
        });

      mockDeduction
        .Setup(d => d.computeDeductions(It.IsAny<List<PayrollEmployeeModel>>()))
        .ReturnsAsync(new List<PayrollEmployeeModel> {
          new PayrollEmployeeModel {
            id = employeeId,
            ccssEmployeeDeduction = 50,
            computedGrossSalary = 1000,
            gender = "",
            birthDate = DateOnly.MinValue,
            rentTax = 0,
            rawGrossSalary = 0,
            ccssEmployerDeduction = 0,
            hoursDate = DateOnly.MinValue,
            hoursNumber = 0,
            companyAssociation = "",
            previousComputedGrossSalaries =
              new List<PayrollPreviousComputedGrossSalary>(),
            fullName = "",
            hiringDate = new DateOnly(2020, 1, 1),
            hiringType = "tiempoCompleto",
            deductions = new List<PayrollDeductionModel> {
              new PayrollDeductionModel {
                resultAmount = 300,
                id = "",
                dependantNumber = 0,
                formulaType = "",
                apiUrl = "",
                apiMethod = "",
                param1Value = "",
                param2Value = "",
                param3Value = "",
                param1Key = "",
                param2Key = "",
                param3Key = "",
                header1Key = "",
                header1Value = "",
              },
              new PayrollDeductionModel {
                resultAmount = 600,
                id = "",
                dependantNumber = 0,
                formulaType = "",
                apiUrl = "",
                apiMethod = "",
                param1Value = "",
                param2Value = "",
                param3Value = "",
                param1Key = "",
                param2Key = "",
                param3Key = "",
                header1Key = "",
                header1Value = "",
              }
          },
          }
        });

      var orchestrator = new PayrollOrchestrator(
        mockPayrollEmployee.Object,
        mockGrossSalary.Object,
        mockRentTax.Object,
        mockCCSS.Object,
        mockDeduction.Object
      );

      var result = await orchestrator.ComputePayrollAsync("EMP123"
        , new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 31));

      Assert.That(result, Has.Count.EqualTo(1));

      var r = result[0];
      Assert.That(r.ComputedGrossSalary, Is.EqualTo(1000));
      Assert.That(r.RentTax, Is.EqualTo(100));
      Assert.That(r.CcssTax, Is.EqualTo(50));
      Assert.That(r.TotalDeductions, Is.EqualTo(100 + 50 + 300));
      Assert.That(r.NetSalary, Is.EqualTo(1000 - 100 - 50 - 300));
      Assert.That(r.Deductions, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task ComputePayrollAsync_ShouldNotApplyBenefitDeductions_WhenNetIsInsufficient()
    {
      var mockPayrollEmployee = new Mock<IPayrollEmployee>();
      var mockGrossSalary = new Mock<IGrossSalary>();
      var mockRentTax = new Mock<IRentTax>();
      var mockCCSS = new Mock<ITaxCCSS>();
      var mockDeduction = new Mock<IDeduction>();

      var employeeId = Guid.NewGuid().ToString() + " abc";

      var employee = new PayrollEmployeeModel
      {
        id = employeeId,
        fullName = "",
        hiringDate = new DateOnly(2021, 5, 1),
        hiringType = "tiempoCompleto",
        computedGrossSalary = 400,

        rentTax = 0,
        gender = "",
        birthDate = DateOnly.MinValue,
        rawGrossSalary = 0,
        ccssEmployeeDeduction = 0,
        ccssEmployerDeduction = 0,
        hoursDate = DateOnly.MinValue,
        hoursNumber = 0,
        companyAssociation = "",
        deductions = new List<PayrollDeductionModel>(),
        previousComputedGrossSalaries =
          new List<PayrollPreviousComputedGrossSalary>(),
      };

      mockPayrollEmployee
          .Setup(p => p.getPayrollEmployees("EMP999", It.IsAny<DateOnly>()
            , It.IsAny<DateOnly>()))
          .Returns(new List<PayrollEmployeeModel> { employee });

      mockGrossSalary
          .Setup(g =>
            g.computeAllGrossSalaries(It.IsAny<List<PayrollEmployeeModel>>()
            , It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
          .Returns(new List<PayrollEmployeeModel> { employee });

      mockRentTax
          .Setup(r =>
            r.calculateRentTaxes(It.IsAny<List<PayrollEmployeeModel>>(),
            It.IsAny<DateOnly>()))
          .Returns(new List<PayrollEmployeeModel> {
            new PayrollEmployeeModel {
                id = employeeId,
                rentTax = 100,
                computedGrossSalary = 400,

                gender = "",
                birthDate = DateOnly.MinValue,
                rawGrossSalary = 0,
                ccssEmployeeDeduction = 0,
                ccssEmployerDeduction = 0,
                hoursDate = DateOnly.MinValue,
                hoursNumber = 0,
                companyAssociation = "",
                deductions = new List<PayrollDeductionModel>(),
                previousComputedGrossSalaries =
                  new List<PayrollPreviousComputedGrossSalary>(),
                fullName = "",
                hiringDate = new DateOnly(2020, 1, 1),
                hiringType = "tiempoCompleto"
            }
          });

      mockCCSS
          .Setup(c => c.computeTaxesCCSS(It.IsAny<List<PayrollEmployeeModel>>()
            , It.IsAny<DateOnly>()))
          .Returns(new List<PayrollEmployeeModel> {
            new PayrollEmployeeModel {
                id = employeeId,
                ccssEmployeeDeduction = 50,
                computedGrossSalary = 400,

                rentTax = 0,
                gender = "",
                birthDate = DateOnly.MinValue,
                rawGrossSalary = 0,
                ccssEmployerDeduction = 0,
                hoursDate = DateOnly.MinValue,
                hoursNumber = 0,
                companyAssociation = "",
                deductions = new List<PayrollDeductionModel>(),
                previousComputedGrossSalaries =
                  new List<PayrollPreviousComputedGrossSalary>(),
                fullName = "",
                hiringDate = new DateOnly(2020, 1, 1),
            hiringType = "tiempoCompleto"
            }
          });

      mockDeduction
          .Setup(d =>
            d.computeDeductions(It.IsAny<List<PayrollEmployeeModel>>()))
          .ReturnsAsync(new List<PayrollEmployeeModel> {
            new PayrollEmployeeModel {
                id = employeeId,
                computedGrossSalary = 400,
                ccssEmployeeDeduction = 50,

                rentTax = 0,
                gender = "",
                birthDate = DateOnly.MinValue,
                rawGrossSalary = 0,
                ccssEmployerDeduction = 0,
                hoursDate = DateOnly.MinValue,
                hoursNumber = 0,
                companyAssociation = "",
                previousComputedGrossSalaries =
                  new List<PayrollPreviousComputedGrossSalary>(),
                fullName = "",
                hiringDate = new DateOnly(2020, 1, 1),
                hiringType = "tiempoCompleto",
                deductions = new List<PayrollDeductionModel> {
                    new PayrollDeductionModel {
                      resultAmount = 300,
                      id = "",
                      dependantNumber = 0,
                      formulaType = "",
                      apiUrl = "",
                      apiMethod = "",
                      param1Value = "",
                      param2Value = "",
                      param3Value = "",
                      param1Key = "",
                      param2Key = "",
                      param3Key = "",
                      header1Key = "",
                      header1Value = "",
                    },
                    new PayrollDeductionModel {
                      resultAmount = 600,
                      id = "",
                      dependantNumber = 0,
                      formulaType = "",
                      apiUrl = "",
                      apiMethod = "",
                      param1Value = "",
                      param2Value = "",
                      param3Value = "",
                      param1Key = "",
                      param2Key = "",
                      param3Key = "",
                      header1Key = "",
                      header1Value = "",
                    }
                }
            }
          });

      var orchestrator = new PayrollOrchestrator(
          mockPayrollEmployee.Object,
          mockGrossSalary.Object,
          mockRentTax.Object,
          mockCCSS.Object,
          mockDeduction.Object
      );

      var result = await orchestrator.ComputePayrollAsync("EMP999",
          new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 31));

      Assert.That(result, Has.Count.EqualTo(1));
      var r = result[0];

      Assert.That(r.ComputedGrossSalary, Is.EqualTo(400));
      Assert.That(r.RentTax, Is.EqualTo(100));
      Assert.That(r.CcssTax, Is.EqualTo(50));
      Assert.That(r.TotalDeductions, Is.EqualTo(150));
      Assert.That(r.NetSalary, Is.EqualTo(250));
      Assert.That(r.Deductions, Has.Count.EqualTo(0));
    }
  }
}
