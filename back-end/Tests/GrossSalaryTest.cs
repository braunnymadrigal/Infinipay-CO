using System;
using System.Collections.Generic;
using System.Linq;
using back_end.Application;
using back_end.Domain;

namespace Tests
{
    class GrossSalaryTest
    {
        IStrategyGrossSalaryComputation weekly;
        IStrategyGrossSalaryComputation biweekly;
        IStrategyGrossSalaryComputation monthly;
        List<PayrollEmployeeModel> employees;
        DateOnly startDate;
        DateOnly endDate;

        [SetUp]
        public void Setup()
        {
            weekly = new WeeklyGrossSalaryComputation();
            biweekly = new BiweeklyGrossSalaryComputation();
            monthly = new MonthlyGrossSalaryComputation();
            employees = new List<PayrollEmployeeModel> {
                new PayrollEmployeeModel
                {
                    id = "",
                    gender = "",
                    birthDate = DateOnly.MinValue,
                    rentTax = 0.0,
                    rawGrossSalary = 0.0,
                    computedGrossSalary = 0.0,
                    ccssEmployeeDeduction = 0.0,
                    ccssEmployerDeduction = 0.0,
                    hiringDate = DateOnly.MinValue,
                    hiringType = "",
                    hoursDate = DateOnly.MinValue,
                    hoursNumber = 0,
                    companyAssociation = "",
                    deductions = new List<PayrollDeductionModel>(),
                    previousComputedGrossSalaries = new List<PayrollPreviousComputedGrossSalary>()
                }
            };
        }

        [Test]
        public void Test_WeeklyGrossSalaryComputation_0Hours()
        {
            // Arrange
            employees[0].hoursNumber = 0;
            var expectedResult = 0.0;

            // Act
            var resultList = weekly.ComputeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            // Assert
            Assert.That(expectedResult, Is.EqualTo(result));
        }

        //[Test]
        //public void Test_Print_Stub()
        //{
        //    // Arrange
        //    var numTimes = 3;
        //    var stub = new Mock<IDependency>();
        //    stub.Setup(d => d.ImportantMethod(numTimes)).Returns("Hello");

        //    var printer = new Printer(stub.Object);
        //    var expectedResult = "print:HelloHelloHello";

        //    // Act
        //    var result = printer.Print(numTimes);

        //    // Assert
        //    Assert.AreEqual(expectedResult, result);
        //}

        //[Test]
        //public void Test_Print_Mock()
        //{
        //    // Arrange
        //    var numTimes = 3;
        //    var mock = new Mock<IDependency>();
        //    mock.Setup(d => d.ImportantMethod(numTimes)).Returns("Hello");

        //    var printer = new Printer(mock.Object);
        //    var expectedResult = "print:HelloHelloHello";

        //    // Act
        //    var result = printer.Print(numTimes);

        //    // Assert
        //    Assert.AreEqual(expectedResult, result);
        //    mock.Verify(d => d.ImportantMethod(numTimes), Times.Exactly(numTimes));
        //}
    }
}
