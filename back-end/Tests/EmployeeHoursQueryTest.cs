using NUnit.Framework;
using Moq;
using back_end.Application;
using back_end.Domain;
using back_end.Infraestructure;
using System;
using System.Collections.Generic;
using System.IO;

namespace Tests
{
  public class EmployeeHoursQueryTests
  {
    private Mock<IEmployeeHoursRepository> mockRepository;
    private EmployeeHoursQuery query;

    [SetUp]
    public void Setup()
    {
      mockRepository = new Mock<IEmployeeHoursRepository>();
      query = new EmployeeHoursQuery(mockRepository.Object);
    }

    [Test]
    public void GetEmployeeHoursContract_ShouldReturnModel_WhenValid()
    {
      var loggedUserId = "daniel231";
      var expected = new EmployeeHoursModel();

      mockRepository.Setup(r => r.getEmployeeHoursContract(loggedUserId))
          .Returns(expected);

      var result = query.getEmployeeHoursContract(loggedUserId);

      Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void GetEmployeeHoursContract_ShouldThrow_WhenUserIdIsNull()
    {
      var error = Assert.Throws<ArgumentNullException>(() =>
          query.getEmployeeHoursContract(null));

      Assert.That(error.ParamName, Is.EqualTo("loggedUserId"));
    }

    [Test]
    public void GetEmployeeHoursContract_ShouldThrow_WhenResultIsNull()
    {
      var loggedUserId = "daniel231";

      mockRepository.Setup(r => r.getEmployeeHoursContract(loggedUserId))
          .Returns((EmployeeHoursModel)null);

      var error = Assert.Throws<InvalidDataException>(() =>
          query.getEmployeeHoursContract(loggedUserId));

      Assert.That(error.Message, Does.Contain("No se puedo obtener detalles"));
    }

    [Test]
    public void GetEmployeeHoursList_ShouldReturnList_WhenValid()
    {
      var loggedUserId = "daniel231";
      var start = new DateOnly(2024, 6, 1);
      var end = new DateOnly(2024, 6, 7);
      var expected = new List<HoursModel>
            {
                new HoursModel { date = start, hoursWorked = 8 }
            };

      mockRepository.Setup(r => r.getEmployeeHoursList(loggedUserId, start
        , end)).Returns(expected);

      var result = query.getEmployeeHoursList(loggedUserId, start, end);

      Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void GetEmployeeHoursList_ShouldThrow_WhenUserIdIsNull()
    {
      var start = new DateOnly(2024, 6, 1);
      var end = new DateOnly(2024, 6, 7);

      var error = Assert.Throws<ArgumentNullException>(() =>
          query.getEmployeeHoursList(null, start, end));

      Assert.That(error.ParamName, Is.EqualTo("loggedUserId"));
    }

    [Test]
    public void GetEmployeeHoursList_ShouldThrow_WhenStartDateAfterEndDate()
    {
      var loggedUserId = "daniel231";
      var start = new DateOnly(2024, 6, 8);
      var end = new DateOnly(2024, 6, 7);

      var error = Assert.Throws<InvalidDataException>(() =>
          query.getEmployeeHoursList(loggedUserId, start, end));

      Assert.That(error.Message, Does.Contain("Fecha de inicio"));
    }

    [Test]
    public void GetEmployeeHoursList_ShouldThrow_WhenRepositoryReturnsNull()
    {
      var loggedUserId = "daniel231";
      var start = new DateOnly(2024, 6, 1);
      var end = new DateOnly(2024, 6, 7);

      mockRepository.Setup(r => r.getEmployeeHoursList(loggedUserId, start
        , end)).Returns((List<HoursModel>)null);

      var error = Assert.Throws<InvalidDataException>(() =>
          query.getEmployeeHoursList(loggedUserId, start, end));

      Assert.That(error.Message, Does.Contain("Horas no obtenidas"));
    }
  }
}
