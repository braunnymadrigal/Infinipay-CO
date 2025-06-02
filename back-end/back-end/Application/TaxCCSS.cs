using System.Xml.Serialization;
using back_end.Domain;

namespace back_end.Application
{
    public class TaxCCSS : ITaxCCSS
    {
        private const double P_VALUE_ON_DECIMAL_SQL_TYPE = 11.0;
        private const double S_VALUE_ON_DECIMAL_SQL_TYPE = 2.0;
        private const double EXPONENTATION_BASE_VALUE_ON_DECIMAL_SQL_TYPE = 10.0;
        private const double EMPLOYER_TAX_PERCENT = 0.1467;
        private const double EMPLOYEE_TAX_PERCENT = 0.0967;
        private const double MINIMUM_MONTHLY_CONTRIBUTION = 163669.0;

        private const int DAYS_IN_A_WEEK = 7;
        private const int BIWEEKLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK = 15;
        private const int MONTHLY_EMPLOYEE_MAXIMUM_DAYS_OF_WORK = 30;
        private const int MONTHS_TO_SUBSTRACT = -1;

        private const string HIRING_TYPE_EXCLUDED_FROM_TAXES = "servicios";

        public List<PayrollEmployeeModel> computeTaxesCCSS(List<PayrollEmployeeModel> 
            payrollEmployees, DateOnly endDate)
        {
            for (int i = 0; i < payrollEmployees.Count; ++i)
            {
                payrollEmployees[i] = computeTaxCCSS(payrollEmployees[i], endDate);
            }
            return payrollEmployees;
        }

        private PayrollEmployeeModel computeTaxCCSS(PayrollEmployeeModel payrollEmployee, 
            DateOnly endDate)
        {
            validateComputedGrossSalary(payrollEmployee.computedGrossSalary);
            var employeeTax = 0.0;
            var employerTax = 0.0;
            if (payrollEmployee.hiringType != HIRING_TYPE_EXCLUDED_FROM_TAXES)
            {
                var grossSalary = payrollEmployee.computedGrossSalary;
                if (isTheEndOfTheMonth(endDate))
                {
                    var sumOfSalaries = sumPreviousSalaries(
                        payrollEmployee.previousComputedGrossSalaries, 
                        endDate.AddMonths(MONTHS_TO_SUBSTRACT));
                    if (sumOfSalaries + grossSalary < MINIMUM_MONTHLY_CONTRIBUTION)
                    {
                        grossSalary = (MINIMUM_MONTHLY_CONTRIBUTION - sumOfSalaries);
                    }
                }
                employeeTax = grossSalary * EMPLOYEE_TAX_PERCENT;
                employerTax = grossSalary * EMPLOYER_TAX_PERCENT;
            }
            payrollEmployee.ccssEmployeeDeduction = employeeTax;
            payrollEmployee.ccssEmployerDeduction = employerTax;
            return payrollEmployee;
        }

        private double sumPreviousSalaries(List<PayrollPreviousComputedGrossSalary> salaries, 
            DateOnly minimumDate)
        {
            var sum = 0.0;
            foreach (var salary in salaries)
            {
                if (salary.startDate > minimumDate)
                {
                    sum += salary.amount;
                }
            }
            return sum;
        }

        private bool isTheEndOfTheMonth(DateOnly endDate)
        {
            return !(endDate.Month == endDate.AddDays(DAYS_IN_A_WEEK).Month);
        }

        private void validateComputedGrossSalary(double computedGrossSalary)
        {
            validateComputedGrossSalaryGreaterThanZero(computedGrossSalary);
            validateComputedGrossSalarySize(computedGrossSalary);
        }

        private void validateComputedGrossSalarySize(double computedGrossSalary)
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

        private void validateComputedGrossSalaryGreaterThanZero(double computedGrossSalary)
        {
            if (computedGrossSalary < 0)
            {
                throw new Exception("The computed gross salary can not be less than zero.");
            }
        }
    }
}
