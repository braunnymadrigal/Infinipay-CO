using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using back_end.Application;
using back_end.Domain;

namespace Tests
{
    class BenefitDeductionsTest
    {
        private const string GEEMS_API_URL =
            "https://asociacion-geems-c3dfavfsapguhxbp.southcentralus-01.azurewebsites.net/api/public/calculator/calculate";

        IDeduction deduction;
        List<PayrollEmployeeModel> employees;

        [SetUp]
        public void Setup()
        {
            deduction = new Deduction();
            employees = new List<PayrollEmployeeModel> 
            {
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
                    deductions = new List<PayrollDeductionModel>
                    {
                        new PayrollDeductionModel
                        {
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
                            resultAmount = 0.0
                        }
                    },
                    previousComputedGrossSalaries = new List<PayrollPreviousComputedGrossSalary>()
                }
            };
        }

        [Test]
        public void Test_Deduction_InvalidFormula()
        {
            employees[0].deductions[0].formulaType = "a";
            Assert.ThrowsAsync<Exception>(async () =>
            {
                await deduction.computeDeductions(employees);
            });
        }

        [Test]
        public async Task Test_Deduction_FixedAmountValid()
        {
            employees[0].deductions[0].formulaType = "montoFijo";
            employees[0].deductions[0].param1Value = "2500.50";
            var expectedResult = 2500.50;
            var resultList = await deduction.computeDeductions(employees);
            var result = resultList[0].deductions[0].resultAmount;
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Test_Deduction_FixedAmountNotNumber()
        {
            employees[0].deductions[0].formulaType = "montoFijo";
            employees[0].deductions[0].param1Value = "abc";
            Assert.ThrowsAsync<Exception>(async () =>
            {
                await deduction.computeDeductions(employees);
            });
        }

        [Test]
        public void Test_Deduction_FixedAmountNegative()
        {
            employees[0].deductions[0].formulaType = "montoFijo";
            employees[0].deductions[0].param1Value = "-100";
            Assert.ThrowsAsync<Exception>(async () =>
            {
                await deduction.computeDeductions(employees);
            });
        }

        [Test]
        public async Task Test_Deduction_PercentageValid()
        {
            employees[0].computedGrossSalary = 1000.0;
            employees[0].deductions[0].formulaType = "porcentaje";
            employees[0].deductions[0].param1Value = "10";
            var expectedResult = 100.0;
            var resultList = await deduction.computeDeductions(employees);
            var result = resultList[0].deductions[0].resultAmount;
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Test_Deduction_PercentageNotNumber()
        {
            employees[0].computedGrossSalary = 1000.0;
            employees[0].deductions[0].formulaType = "porcentaje";
            employees[0].deductions[0].param1Value = "notANumber";
            Assert.ThrowsAsync<Exception>(async () =>
            {
                await deduction.computeDeductions(employees);
            });
        }

        [Test]
        public void Test_Deduction_PercentageNegative()
        {
            employees[0].computedGrossSalary = 1000.0;
            employees[0].deductions[0].formulaType = "porcentaje";
            employees[0].deductions[0].param1Value = "-2.5";
            Assert.ThrowsAsync<Exception>(async () =>
            {
                await deduction.computeDeductions(employees);
            });
        }

        [Test]
        public void Test_Deduction_ApiInvalidUrl()
        {
            employees[0].deductions[0].formulaType = "api";
            employees[0].deductions[0].apiUrl = "a";
            Assert.ThrowsAsync<Exception>(async () =>
            {
                await deduction.computeDeductions(employees);
            });
        }

        [Test]
        public async Task Test_Deduction_ApiValid()
        {
            employees[0].deductions[0].formulaType = "api";
            employees[0].deductions[0].apiUrl = GEEMS_API_URL;
            employees[0].deductions[0].header1Key = "API-KEY";
            employees[0].deductions[0].header1Value = "Tralalerotralala";
            employees[0].deductions[0].param1Key = "associationName";
            employees[0].deductions[0].param2Key = "employeeSalary";
            employees[0].deductions[0].resultAmount = -777.777;
            employees[0].computedGrossSalary = 30000.25;
            employees[0].companyAssociation = "a";
            var resultList = await deduction.computeDeductions(employees);
            var result = resultList[0].deductions[0].resultAmount;
            Assert.That(result, Is.GreaterThan(-777.777));
        }
    }
}
