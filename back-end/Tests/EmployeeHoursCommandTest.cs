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
  public class EmployeeHoursCommandTests
  {
    private Mock<IEmployeeHoursRepository> mockRepository;
    private EmployeeHoursCommand command;

    [SetUp]
    public void Setup()
    {
      mockRepository = new Mock<IEmployeeHoursRepository>();
      command = new EmployeeHoursCommand(mockRepository.Object);
    }

    [Test]
    public void Should_RegisterEmployeeHours_WhenDataIsValid()
    {
      var loggedUserId = "daniel231";
      var hoursWorked = new List<HoursModel>
            {
                new HoursModel { date = DateOnly.FromDateTime(DateTime.Now)
                , hoursWorked = 8 }
            };

      mockRepository.Setup(r => r.registerEmployeeHours(loggedUserId
        , hoursWorked)).Returns(true);

      var result = command.registerEmployeeHours(loggedUserId, hoursWorked);

      Assert.IsTrue(result);
    }

    [Test]
    public void Should_Throw_WhenUserIdIsNull()
    {
      var hoursWorked = new List<HoursModel>
            {
                new HoursModel { date = DateOnly.FromDateTime(DateTime.Now)
                , hoursWorked = 8 }
            };

      var error = Assert.Throws<Exception>(()
        => command.registerEmployeeHours(null, hoursWorked));

      Assert.That(error.Message, Does.Contain(
        "Error en datos de horas: Value cannot be null"));

      Assert.That(error.InnerException, Is.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void Should_Throw_WhenListIsEmpty()
    {
      var error = Assert.Throws<Exception>(() => command.registerEmployeeHours(
        "daniel231", new List<HoursModel>())
      );

      Assert.That(error.Message, Does.Contain("No hay horas por registrar"));
    }

    [Test]
    public void Should_Throw_WhenAnyEntryIsNull()
    {
      var hoursWorked = new List<HoursModel> { null };

      var error = Assert.Throws<Exception>(() => command.registerEmployeeHours(
        "daniel231", hoursWorked));

      Assert.That(error.Message, Does.Contain("Horas registradas invalidas"));
    }

    [Test]
    public void Should_Throw_WhenDateIsInFuture()
    {
      var hoursWorked = new List<HoursModel>
            {
                new HoursModel { date = DateOnly.FromDateTime(
                  DateTime.Now.AddDays(1)), hoursWorked = 6 }
            };

      var error = Assert.Throws<Exception>(() => command.registerEmployeeHours(
        "daniel231", hoursWorked));
      Assert.That(error.Message, Does.Contain("Fecha de registro invalido"));
    }

    [Test]
    public void Should_Throw_WhenHoursOver9()
    {
      var hoursWorked = new List<HoursModel>
            {
                new HoursModel { date = DateOnly.FromDateTime(DateTime.Now)
                , hoursWorked = 10 }
            };

      var error = Assert.Throws<Exception>(() => command.registerEmployeeHours(
        "daniel231", hoursWorked));
      Assert.That(error.Message, Does.Contain(
        "Cantidad de horas registradas invalidas"));
    }

    [Test]
    public void Should_Throw_WhenHoursBelow1()
    {
      var hoursWorked = new List<HoursModel>
            {
                new HoursModel { date = DateOnly.FromDateTime(DateTime.Now)
                , hoursWorked = -10 }
            };

      var error = Assert.Throws<Exception>(() => command.registerEmployeeHours(
        "daniel231", hoursWorked));

      Assert.That(error.Message, Does.Contain(
        "Cantidad de horas registradas invalidas"));
    }
  }
}
