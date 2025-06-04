using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using back_end.Application;
using back_end.Domain;

namespace Tests
{
    class CCSSTaxTest
    {
        private const double EMPLOYEE_TAX = 0.0967;
        private const double EMPLOYER_TAX = 0.1467;
        private const double MINIMUM_CONTRIBUTION_BASE = 163669.0;
        ITaxCCSS taxCCSS;
        List<PayrollEmployeeModel> employees;
        DateOnly endDate;

        [SetUp]
        public void Setup()
        {
            taxCCSS = new TaxCCSS();
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
        public void Test_CCSSTaxComputation_NegativeSalary()
        {
            employees[0].computedGrossSalary = -100.0;
            endDate = new DateOnly(2025, 6, 30);
            Assert.Throws<Exception>(() => taxCCSS.computeTaxesCCSS(employees, endDate));
        }

        [Test]
        public void Test_CCSSTaxComputation_TooBigSalary()
        {
            employees[0].computedGrossSalary = 9999999999.99;
            endDate = new DateOnly(2025, 6, 30);
            Assert.Throws<Exception>(() => taxCCSS.computeTaxesCCSS(employees, endDate));
        }

        [Test]
        public void Test_CCSSTaxComputation_ByServices()
        {
            employees[0].computedGrossSalary = 1000.0;
            employees[0].hiringType = "servicios";
            endDate = new DateOnly(2025, 6, 15);
            var resultList = taxCCSS.computeTaxesCCSS(employees, endDate);
            var result = resultList[0];
            Assert.That(result.ccssEmployeeDeduction, Is.EqualTo(0.0));
            Assert.That(result.ccssEmployerDeduction, Is.EqualTo(0.0));
        }

        [Test]
        public void Test_CCSSTaxComputation_NormalSalary()
        {
            employees[0].computedGrossSalary = 1000.0;
            endDate = new DateOnly(2025, 6, 15);
            var expectedEmployeeTax = 1000.0 * EMPLOYEE_TAX;
            var expectedEmployerTax = 1000.0 * EMPLOYER_TAX;
            var resultList = taxCCSS.computeTaxesCCSS(employees, endDate);
            var result = resultList[0];
            Assert.That(result.ccssEmployeeDeduction, Is.EqualTo(expectedEmployeeTax).Within(0.01));
            Assert.That(result.ccssEmployerDeduction, Is.EqualTo(expectedEmployerTax).Within(0.01));
        }

        [Test]
        public void Test_CCSSTaxComputation_EndOfMonthForceMinimum()
        {
            employees[0].computedGrossSalary = 100000.0;
            employees[0].previousComputedGrossSalaries = new List<PayrollPreviousComputedGrossSalary>
            {
                new PayrollPreviousComputedGrossSalary
                {
                    startDate = new DateOnly(2025, 6, 10),
                    amount = 20000.0
                },
                new PayrollPreviousComputedGrossSalary
                {
                    startDate = new DateOnly(2025, 6, 20),
                    amount = 10000.0
                }
            };
            endDate = new DateOnly(2025, 6, 30);
            var adjustedGross = MINIMUM_CONTRIBUTION_BASE - (20000.0 + 10000.0);
            var expectedEmployeeTax = adjustedGross * EMPLOYEE_TAX;
            var expectedEmployerTax = adjustedGross * EMPLOYER_TAX;
            var resultList = taxCCSS.computeTaxesCCSS(employees, endDate);
            var result = resultList[0];
            Assert.That(result.ccssEmployeeDeduction, Is.EqualTo(expectedEmployeeTax).Within(0.01));
            Assert.That(result.ccssEmployerDeduction, Is.EqualTo(expectedEmployerTax).Within(0.01));
        }

        [Test]
        public void Test_CCSSTaxComputation_EndOfMonthAlreadyMeetsMinimum()
        {
            employees[0].computedGrossSalary = 140000.0;
            employees[0].hiringType = "planilla";
            employees[0].previousComputedGrossSalaries = new List<PayrollPreviousComputedGrossSalary>
            {
                new PayrollPreviousComputedGrossSalary
                {
                    startDate = new DateOnly(2025, 6, 20),
                    amount = 30000.0
                }
            };
            endDate = new DateOnly(2025, 6, 30);
            var expectedEmployeeTax = 140000.0 * EMPLOYEE_TAX;
            var expectedEmployerTax = 140000.0 * EMPLOYER_TAX;
            var resultList = taxCCSS.computeTaxesCCSS(employees, endDate);
            var result = resultList[0];
            Assert.That(result.ccssEmployeeDeduction, Is.EqualTo(expectedEmployeeTax).Within(0.01));
            Assert.That(result.ccssEmployerDeduction, Is.EqualTo(expectedEmployerTax).Within(0.01));
        }
    }
}
