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

        [Test]
        public void Test_WeeklyGrossSalaryComputation_ZeroRawSalary()
        {
            // Arrange
            employees[0].rawGrossSalary = 0.0;
            employees[0].hoursNumber = 40;
            var expectedResult = 0.0;

            // Act
            var resultList = weekly.ComputeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Test_WeeklyGrossSalaryComputation_OneHour()
        {
            // Arrange
            employees[0].rawGrossSalary = 25.0;
            employees[0].hoursNumber = 1;
            var expectedResult = 25.0;

            // Act
            var resultList = weekly.ComputeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Test_WeeklyGrossSalaryComputation_HighHours()
        {
            // Arrange
            employees[0].rawGrossSalary = 10.0;
            employees[0].hoursNumber = 100;
            var expectedResult = 1000.0;

            // Act
            var resultList = weekly.ComputeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Test_WeeklyGrossSalaryComputation_DecimalPrecision()
        {
            // Arrange
            employees[0].rawGrossSalary = 13.333;
            employees[0].hoursNumber = 3;
            var expectedResult = 39.999;

            // Act
            var resultList = weekly.ComputeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult).Within(0.001));
        }

        [Test]
        public void Test_WeeklyGrossSalaryComputation_NegativeHours()
        {
            // Arrange
            employees[0].rawGrossSalary = 20.0;
            employees[0].hoursNumber = -5;
            var expectedResult = -100.0;

            // Act
            var resultList = weekly.ComputeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Test_BiweeklyGrossSalaryComputation_HiredBeforeStartDate()
        {
            // Arrange
            employees[0].rawGrossSalary = 1000.0;
            employees[0].hiringDate = new DateOnly(2020, 1, 1);
            startDate = new DateOnly(2025, 6, 1);
            endDate = new DateOnly(2025, 6, 15);
            var expectedResult = 500.0;

            // Act
            var resultList = biweekly.ComputeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Test_BiweeklyGrossSalaryComputation_HiredOnStartDate()
        {
            // Arrange
            employees[0].rawGrossSalary = 900.0;
            employees[0].hiringDate = new DateOnly(2025, 6, 1);
            startDate = new DateOnly(2025, 6, 1);
            endDate = new DateOnly(2025, 6, 15);
            var expectedResult = 450.0;

            // Act
            var resultList = biweekly.ComputeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Test_BiweeklyGrossSalaryComputation_HiredAfterStartDate()
        {
            // Arrange
            employees[0].rawGrossSalary = 900.0;
            employees[0].hiringDate = new DateOnly(2025, 6, 10);
            startDate = new DateOnly(2025, 6, 1);
            endDate = new DateOnly(2025, 6, 15);
            var expectedWorkedDays = 6;
            var expectedResult = (900.0 / 15.0) * expectedWorkedDays;

            // Act
            var resultList = biweekly.ComputeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Test_BiweeklyGrossSalaryComputation_HiredOnLastDay()
        {
            // Arrange
            employees[0].rawGrossSalary = 600.0;
            employees[0].hiringDate = new DateOnly(2025, 6, 15);
            startDate = new DateOnly(2025, 6, 1);
            endDate = new DateOnly(2025, 6, 15);
            var expectedResult = (600.0 / 15.0);

            // Act
            var resultList = biweekly.ComputeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Test_BiweeklyGrossSalaryComputation_DecimalPrecision()
        {
            // Arrange
            employees[0].rawGrossSalary = 1234.56;
            employees[0].hiringDate = new DateOnly(2025, 6, 5);
            startDate = new DateOnly(2025, 6, 1);
            endDate = new DateOnly(2025, 6, 15);
            var expectedWorkedDays = 11;
            var expectedResult = (1234.56 / 15.0) * expectedWorkedDays;

            // Act
            var resultList = biweekly.ComputeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult).Within(0.01));
        }

        [Test]
        public void Test_MonthlyGrossSalaryComputation_HiredBeforeStartDate()
        {
            // Arrange
            employees[0].rawGrossSalary = 1500.0;
            employees[0].hiringDate = new DateOnly(2020, 1, 1);
            startDate = new DateOnly(2025, 6, 1);
            endDate = new DateOnly(2025, 6, 30);
            var expectedResult = 1500.0;

            // Act
            var resultList = monthly.ComputeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Test_MonthlyGrossSalaryComputation_HiredOnStartDate()
        {
            // Arrange
            employees[0].rawGrossSalary = 1800.0;
            employees[0].hiringDate = new DateOnly(2025, 6, 1);
            startDate = new DateOnly(2025, 6, 1);
            endDate = new DateOnly(2025, 6, 30);
            var expectedResult = 1800.0;

            // Act
            var resultList = monthly.ComputeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Test_MonthlyGrossSalaryComputation_HiredAfterStartDate()
        {
            // Arrange
            employees[0].rawGrossSalary = 2100.0;
            employees[0].hiringDate = new DateOnly(2025, 6, 10);
            startDate = new DateOnly(2025, 6, 1);
            endDate = new DateOnly(2025, 6, 30);
            var expectedWorkedDays = 21;
            var expectedResult = (2100.0 / 30.0) * expectedWorkedDays;

            // Act
            var resultList = monthly.ComputeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Test_MonthlyGrossSalaryComputation_HiredOnLastDay()
        {
            // Arrange
            employees[0].rawGrossSalary = 3000.0;
            employees[0].hiringDate = new DateOnly(2025, 6, 30);
            startDate = new DateOnly(2025, 6, 1);
            endDate = new DateOnly(2025, 6, 30);
            var expectedResult = (3000.0 / 30.0);

            // Act
            var resultList = monthly.ComputeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Test_MonthlyGrossSalaryComputation_DecimalPrecision()
        {
            // Arrange
            employees[0].rawGrossSalary = 1789.45;
            employees[0].hiringDate = new DateOnly(2025, 6, 5);
            startDate = new DateOnly(2025, 6, 1);
            endDate = new DateOnly(2025, 6, 30);
            var expectedWorkedDays = 26;
            var expectedResult = (1789.45 / 30.0) * expectedWorkedDays;

            // Act
            var resultList = monthly.ComputeGrossSalary(employees, startDate, endDate);
            var result = resultList[0].computedGrossSalary;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult).Within(0.01));
        }
    }
}
