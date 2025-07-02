using AutoFixture;
using back_end.Application;
using back_end.Infraestructure;
using Microsoft.Extensions.Configuration;
using Moq;
using back_end.Models;
using System.Net.Mail;
using AutoFixture.AutoMoq;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using back_end.Domain;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;

namespace Tests
{
  class AdministratorReportTest
  {
    private AdministratorReportQuery adminReportQuery;
    private Mock<IAdministratorReportRepository> adminReportRepository;

    [SetUp]
    public void Setup()
    {
      adminReportRepository = new Mock<IAdministratorReportRepository>();
      adminReportQuery = new AdministratorReportQuery(adminReportRepository.Object);
    }

    [Test]
    public void getAllCompaniesPayroll_ReturnSuccess_WhenDatesValidAndNoData()
    {
      DateOnly startDate = DateOnly.Parse("01/02/2024");
      DateOnly endDate = DateOnly.Parse("01/02/2025");
      var listLen = 0;

      adminReportRepository
        .Setup(x => x.getAllCompaniesPayroll
        (startDate, endDate))
        .Returns(new List<PayrollAdministratorModel>());

      var result = adminReportQuery.getAllCompaniesPayroll(startDate, endDate);

      adminReportRepository.Verify(x => x.getAllCompaniesPayroll(
        It.IsAny<DateOnly>(), It.IsAny<DateOnly>()), Times.Once);

      Assert.IsNotNull(result);
      Assert.That(result.Count, Is.EqualTo(listLen));
    }

    [Test]
    public void getAllCompaniesPayroll_ReturnSuccess_WhenDatesValidAndHasData()
    {
      DateOnly startDate = DateOnly.Parse("01/02/2024");
      DateOnly endDate = DateOnly.Parse("01/02/2025");

      var expectedPayroll = new List<PayrollAdministratorModel>
    {
        new PayrollAdministratorModel
        {
            companyName = "Acme Inc.",
            hiringType = "quincenal",
            startDate = startDate,
            endDate = endDate,
            payDate = endDate,
            grossSalary = 1000,
            grossEmployerTax = 260,
            voluntaryDeductionsTotal = 50,
            totalEmployerCost = 1260
        },
        new PayrollAdministratorModel
        {
            companyName = "Globex Corp.",
            hiringType = "mensual",
            startDate = startDate,
            endDate = endDate,
            payDate = endDate,
            grossSalary = 2000,
            grossEmployerTax = 0,
            voluntaryDeductionsTotal = 100,
            totalEmployerCost = 2000
        }
    };

      adminReportRepository
          .Setup(r => r.getAllCompaniesPayroll(startDate, endDate))
          .Returns(expectedPayroll);

      var result = adminReportQuery.getAllCompaniesPayroll(startDate, endDate);

      adminReportRepository.Verify(
          r => r.getAllCompaniesPayroll(startDate, endDate),
          Times.Once);

      Assert.IsNotNull(result);
      Assert.That(expectedPayroll.Count, Is.EqualTo(result.Count));
      CollectionAssert.AreEqual(expectedPayroll, result);

    }

    [Test]
    public void getAllCompaniesPayroll_ReturnFailure_WhenFutureDate()
    {
      DateOnly startDate = DateOnly.Parse("01/02/2026");
      DateOnly endDate = DateOnly.Parse("01/02/2027");

      var startDateError = "Fecha inicial invalida.";
      var endDateError = "Fecha final invalida.";

      var expectedPayroll = new List<PayrollAdministratorModel>
    {
        new PayrollAdministratorModel
        {
            companyName = "Acme Inc.",
            hiringType = "quincenal",
            startDate = startDate,
            endDate = endDate,
            payDate = endDate,
            grossSalary = 1000,
            grossEmployerTax = 260,
            voluntaryDeductionsTotal = 50,
            totalEmployerCost = 1260
        },
        new PayrollAdministratorModel
        {
            companyName = "Globex Corp.",
            hiringType = "mensual",
            startDate = startDate,
            endDate = endDate,
            payDate = endDate,
            grossSalary = 2000,
            grossEmployerTax = 0,
            voluntaryDeductionsTotal = 100,
            totalEmployerCost = 2000
        }
    };

      adminReportRepository
          .Setup(r => r.getAllCompaniesPayroll(startDate, endDate))
          .Returns(expectedPayroll);

      var errorMsg = Assert.Throws<Exception>(() =>
                adminReportQuery.getAllCompaniesPayroll(startDate, endDate));

      adminReportRepository.Verify(
          r => r.getAllCompaniesPayroll(startDate, endDate),
          Times.Never);

      Assert.True(errorMsg.Message.Contains(startDateError));
      Assert.True(errorMsg.Message.Contains(endDateError));
    }

    [Test]
    public void
      getAllCompaniesPayroll_ReturnFailure_StartDateGreaterThanEndDate()
    {
      DateOnly startDate = DateOnly.Parse("01/02/2023");
      DateOnly endDate = DateOnly.Parse("01/02/2022");

      var error = "Fecha inicial debe ser menor o igual a la final.";

      var expectedPayroll = new List<PayrollAdministratorModel>
    {
        new PayrollAdministratorModel
        {
            companyName = "Acme Inc.",
            hiringType = "quincenal",
            startDate = startDate,
            endDate = endDate,
            payDate = endDate,
            grossSalary = 1000,
            grossEmployerTax = 260,
            voluntaryDeductionsTotal = 50,
            totalEmployerCost = 1260
        },
        new PayrollAdministratorModel
        {
            companyName = "Globex Corp.",
            hiringType = "mensual",
            startDate = startDate,
            endDate = endDate,
            payDate = endDate,
            grossSalary = 2000,
            grossEmployerTax = 0,
            voluntaryDeductionsTotal = 100,
            totalEmployerCost = 2000
        }
    };

      adminReportRepository
          .Setup(r => r.getAllCompaniesPayroll(startDate, endDate))
          .Returns(expectedPayroll);

      var errorMsg = Assert.Throws<Exception>(() =>
                adminReportQuery.getAllCompaniesPayroll(startDate, endDate));

      adminReportRepository.Verify(
          r => r.getAllCompaniesPayroll(startDate, endDate),
          Times.Never);

      Assert.True(errorMsg.Message.Contains(error));
    }
  }
}
