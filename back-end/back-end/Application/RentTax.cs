using back_end.Domain;

namespace back_end.Application
{
    public class RentTax : IRentTax
    {
        private const double TIER_1_LIMIT = 922_000;
        private const double TIER_2_LIMIT = 1_352_000;
        private const double TIER_3_LIMIT = 2_373_000;
        private const double TIER_4_LIMIT = 4_745_000;

        private const double TIER_2_RATE = 0.10;
        private const double TIER_3_RATE = 0.15;
        private const double TIER_4_RATE = 0.20;
        private const double TIER_5_RATE = 0.25;

        private const int MONTHS_IN_A_YEAR = 12;
        private const string HIRING_TYPE_EXCLUDED_FROM_RENT_TAX = "servicios";

        public List<PayrollEmployeeModel> calculateRentTaxes(List<PayrollEmployeeModel> 
            payrollEmployees, DateOnly endDate)
        {
            for (var i = 0; i < payrollEmployees.Count; ++i)
            {
                if (payrollEmployees[i].hiringType != HIRING_TYPE_EXCLUDED_FROM_RENT_TAX)
                {
                    payrollEmployees[i] = calculateRentTax(payrollEmployees[i], endDate);
                }
            }
            return payrollEmployees;
        }

        private PayrollEmployeeModel calculateRentTax(PayrollEmployeeModel
            payrollEmployee, DateOnly endDate)
        {
            if (isTheEndOfTheMonth(endDate))
            {
                payrollEmployee.taxes.employeeRent = calculateEndOfMonthTax(payrollEmployee);
            }
            else
            {
                payrollEmployee.taxes.employeeRent = calculateProjectedTax(payrollEmployee
                , endDate);
            }
            return payrollEmployee;
        }

        private double calculateEndOfMonthTax(PayrollEmployeeModel employee)
        {
            double accumulatedSalaries = sumPreviousSalaries(employee.previousComputedGrossSalaries);
            double grossSalary = employee.computedGrossSalary + accumulatedSalaries;
            double fullTax = calculateFullRentTax(grossSalary);
            double previousTax = calculateFullRentTax(accumulatedSalaries);
            return fullTax - previousTax;
        }

        private double calculateProjectedTax(PayrollEmployeeModel employee, DateOnly endDate)
        {
            var monthsWorked = Math.Max(1, endDate.Month - employee.hiringDate.Month + 1);
            var projectedAnnualSalary = (employee.computedGrossSalary 
                * MONTHS_IN_A_YEAR) / monthsWorked;
            var projectedTax = calculateFullRentTax(projectedAnnualSalary);
            return projectedTax / MONTHS_IN_A_YEAR;
        }

        private double calculateFullRentTax(double salary)
        {
            var tax = 0.0;
            if (salary > TIER_1_LIMIT)
            {
                tax += calculateTierTax(salary, TIER_1_LIMIT, TIER_2_LIMIT, TIER_2_RATE);
                tax += calculateTierTax(salary, TIER_2_LIMIT, TIER_3_LIMIT, TIER_3_RATE);
                tax += calculateTierTax(salary, TIER_3_LIMIT, TIER_4_LIMIT, TIER_4_RATE);
                tax += calculateExcessTax(salary, TIER_4_LIMIT, TIER_5_RATE);
            }
            return tax;
        }

        private bool isTheEndOfTheMonth(DateOnly endDate)
        {
            return (endDate.Day == DateTime.DaysInMonth(endDate.Year, endDate.Month));
        }

        private double sumPreviousSalaries(List<double> salaries)
        {
            return salaries.Sum(s => s);
        }

        private double calculateTierTax(double salary, double lowerLimit, 
            double upperLimit, double rate)
        {
            var taxableAmount = 0.0;
            if (salary > lowerLimit)
            {
                taxableAmount = Math.Min(salary, upperLimit) - lowerLimit;
            }
            return taxableAmount * rate;
        }

        private double calculateExcessTax(double salary, double lowerLimit
            , double rate)
        {
            var excessTax = 0.0;
            if (salary > lowerLimit)
            {
                excessTax = (salary - lowerLimit) * rate;
            }
            return excessTax;
        }
    }
}
