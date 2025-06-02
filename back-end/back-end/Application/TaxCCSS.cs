using System.Xml.Serialization;
using back_end.Domain;

namespace back_end.Application
{
    public class TaxCCSS : ITaxCCSS
    {
        private const double P_VALUE_ON_DECIMAL_SQL_TYPE = 11.0;
        private const double S_VALUE_ON_DECIMAL_SQL_TYPE = 2.0;
        private const double EXPONENTATION_BASE_VALUE_ON_DECIMAL_SQL_TYPE = 10.0;

        private const string HIRING_TYPE_EXCLUDED_FROM_TAXES = "servicios";

        private const double EMPLOYER_TAX_PERCENT = 0.1467;
        private const double EMPLOYEE_TAX_PERCENT = 0.0967;

        public List<PayrollEmployeeModel> ComputeTaxesCCSS(List<PayrollEmployeeModel> payrollEmployees)
        {
            for (int i = 0; i < payrollEmployees.Count; ++i)
            {
                payrollEmployees[i] = ComputeTaxCCSS(payrollEmployees[i]);
            }
            return payrollEmployees;
        }

        private PayrollEmployeeModel ComputeTaxCCSS(PayrollEmployeeModel payrollEmployee)
        {
            ValidateComputedGrossSalary(payrollEmployee.computedGrossSalary);
            var employeeTax = 0.0;
            var employerTax = 0.0;
            if (payrollEmployee.hiringType != HIRING_TYPE_EXCLUDED_FROM_TAXES)
            {
                employeeTax = payrollEmployee.computedGrossSalary * EMPLOYEE_TAX_PERCENT;
                employerTax = payrollEmployee.computedGrossSalary * EMPLOYER_TAX_PERCENT;
            }
            payrollEmployee.ccssEmployeeDeduction = employeeTax;
            payrollEmployee.ccssEmployerDeduction = employerTax;
            return payrollEmployee;
        }

        private void ValidateComputedGrossSalary(double computedGrossSalary)
        {
            ValidateComputedGrossSalaryGreaterThanZero(computedGrossSalary);
            ValidateComputedGrossSalarySize(computedGrossSalary);
        }

        private void ValidateComputedGrossSalarySize(double computedGrossSalary)
        {
            var maxSize = 
                (Math.Pow(
                    EXPONENTATION_BASE_VALUE_ON_DECIMAL_SQL_TYPE,
                    (P_VALUE_ON_DECIMAL_SQL_TYPE - S_VALUE_ON_DECIMAL_SQL_TYPE)
                    )
                ) - 
                (Math.Pow(EXPONENTATION_BASE_VALUE_ON_DECIMAL_SQL_TYPE, -S_VALUE_ON_DECIMAL_SQL_TYPE));
            if (computedGrossSalary > maxSize)
            {
                throw new Exception("The computed gross salary can not exceed the database limitations");
            }
        }

        private void ValidateComputedGrossSalaryGreaterThanZero(double computedGrossSalary)
        {
            if (computedGrossSalary < 0)
            {
                throw new Exception("The computed gross salary can not be less than zero.");
            }
        }
    }
}
