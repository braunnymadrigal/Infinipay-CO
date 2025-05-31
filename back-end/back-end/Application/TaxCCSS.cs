using System.Xml.Serialization;
using back_end.Domain;

namespace back_end.Application
{
    public class TaxCCSS : ITaxCCSS
    {
        private const int GROSS_SALARIES_MINIMUM_SIZE = 1;
        private const int GROSS_SALARIES_MAXIMUM_SIZE = 1000;

        private const int P_VALUE_ON_DECIMAL_SQL_TYPE = 11;
        private const int S_VALUE_ON_DECIMAL_SQL_TYPE = 2;
        private const int EXPONENTATION_BASE_VALUE_ON_DECIMAL_SQL_TYPE = 10;

        private const string HIRING_TYPE_EXCLUDED_FROM_TAXES = "servicios";

        private const double EMPLOYER_TAX_PERCENT = 0.1467;
        private const double EMPLOYEE_TAX_PERCENT = 0.0967;

        public List<TaxCCSSModel> ComputeTaxesCCSS(List<GrossSalaryModel> grossSalaries)
        {
            var ccssTaxes = new List<TaxCCSSModel>();
            for (int i = 0; i < grossSalaries.Count; ++i)
            {
                var computedResult = ComputeTaxCCSS(grossSalaries[i]);
                ccssTaxes.Add(computedResult);
            }
            return ccssTaxes;
        }

        private TaxCCSSModel ComputeTaxCCSS(GrossSalaryModel grossSalary)
        {
            ValidateComputedGrossSalary(grossSalary.ComputedGrossSalary);
            var employeeTax = 0.0;
            var employerTax = 0.0;
            if (grossSalary.HiringType != HIRING_TYPE_EXCLUDED_FROM_TAXES)
            {
                employeeTax = grossSalary.ComputedGrossSalary * EMPLOYEE_TAX_PERCENT;
                employerTax = grossSalary.ComputedGrossSalary * EMPLOYER_TAX_PERCENT;
            }
            return new TaxCCSSModel { EmployeeAmount = employeeTax, EmployerAmount = employerTax };
        }

        private void ValidateComputedGrossSalary(double computedGrossSalary)
        {
            ValidateComputedGrossSalaryGreaterThanZero(computedGrossSalary);
            ValidateComputedGrossSalarySize(computedGrossSalary);
        }

        private void ValidateComputedGrossSalarySize(double computedGrossSalary)
        {
            var maxSize = (double)(
                EXPONENTATION_BASE_VALUE_ON_DECIMAL_SQL_TYPE ^ 
                (P_VALUE_ON_DECIMAL_SQL_TYPE - S_VALUE_ON_DECIMAL_SQL_TYPE)
                ) - (EXPONENTATION_BASE_VALUE_ON_DECIMAL_SQL_TYPE ^ -S_VALUE_ON_DECIMAL_SQL_TYPE
            );
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

        private void ValidateList(List<GrossSalaryModel> grossSalaries)
        {
            if (grossSalaries.Count < GROSS_SALARIES_MINIMUM_SIZE)
            {
                throw new Exception("The list most contain at least one GrossSalaryModel");
            }
            if (grossSalaries.Count > GROSS_SALARIES_MAXIMUM_SIZE)
            {
                throw new Exception("Due to SqlServer limitations: the list can not exceed 1000 models");
            }
        }
    }
}
