using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using back_end.Application;
using back_end.Domain;

namespace Tests
{
    class BenefitDeductionsTest
    {
        IDeduction deduction;
        List<PayrollEmployeeModel> employees;

        [SetUp]
        public void Setup()
        {
            deduction = new Deduction();
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
    }
}
