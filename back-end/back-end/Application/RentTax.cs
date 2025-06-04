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

    private const string HIRING_TYPE_EXCLUDED_FROM_RENT_TAX = "servicios";

    public List<PayrollEmployeeModel>
      calculateRentTaxes(List<PayrollEmployeeModel> payrollEmployees
      , DateOnly endDate)
    {
      for (int i = 0; i < payrollEmployees.Count; ++i)
      {
        if (payrollEmployees[i].hiringType !=
          HIRING_TYPE_EXCLUDED_FROM_RENT_TAX)
        {
          payrollEmployees[i] = calculateRentTax(payrollEmployees[i], endDate);
        }
      }

      return payrollEmployees;
    }

    private PayrollEmployeeModel calculateRentTax(PayrollEmployeeModel
      payrollEmployee, DateOnly endDate)
    {
      if (isExcludedFromTax(payrollEmployee))
      {
        payrollEmployee.rentTax = 0;
        return payrollEmployee;
      }

      if (isTheEndOfTheMonth(endDate))
      {
        payrollEmployee.rentTax = calculateEndOfMonthTax(payrollEmployee
          , endDate);
      }
      else
      {
        payrollEmployee.rentTax = calculateProjectedTax(payrollEmployee
          , endDate);
      }

      return payrollEmployee;
    }

    private bool isExcludedFromTax(PayrollEmployeeModel employee)
    {
      return employee.hiringType == HIRING_TYPE_EXCLUDED_FROM_RENT_TAX;
    }

    private double calculateEndOfMonthTax(PayrollEmployeeModel employee
      , DateOnly endDate)
    {
      double accumulatedSalaries = sumPreviousSalaries(
          employee.previousComputedGrossSalaries,
          endDate.AddMonths(-1)
      );

      double grossSalary = employee.computedGrossSalary + accumulatedSalaries;

      double fullTax = calculateFullRentTax(grossSalary);

      double previousTax = calculateFullRentTax(accumulatedSalaries);

      return fullTax - previousTax;
    }



    private double calculateProjectedTax(PayrollEmployeeModel employee
      , DateOnly endDate)
    {
      int monthsWorked = Math.Max(1
        , endDate.Month - employee.hiringDate.Month + 1);
      double projectedAnnualSalary = (employee.computedGrossSalary * 12) /
        monthsWorked;
      double projectedTax = calculateFullRentTax(projectedAnnualSalary);
      return projectedTax / 12;
    }

    private double calculateFullRentTax(double salary)
    {
      if (salary <= TIER_1_LIMIT)
        return 0;

      double tax = 0;
      tax += calculateTierTax(salary, TIER_1_LIMIT, TIER_2_LIMIT, TIER_2_RATE);
      tax += calculateTierTax(salary, TIER_2_LIMIT, TIER_3_LIMIT, TIER_3_RATE);
      tax += calculateTierTax(salary, TIER_3_LIMIT, TIER_4_LIMIT, TIER_4_RATE);
      tax += calculateExcessTax(salary, TIER_4_LIMIT, TIER_5_RATE);
      return tax;
    }

    private bool isTheEndOfTheMonth(DateOnly endDate)
    {
      return !(endDate.Month == endDate.AddDays(7).Month);
    }

    private double sumPreviousSalaries(List<PayrollPreviousComputedGrossSalary>
      salaries, DateOnly maximumDate)
    {
      return salaries
          .Where(s => s.startDate <= maximumDate)
          .Sum(s => s.amount);
    }


    private double calculateTierTax(double salary, double lowerLimit
      , double upperLimit, double rate)
    {
      if (salary <= lowerLimit)
        return 0;

      double taxableAmount = Math.Min(salary, upperLimit) - lowerLimit;
      return taxableAmount * rate;
    }

    private double calculateExcessTax(double salary, double lowerLimit
      , double rate)
    {
      if (salary <= lowerLimit)
        return 0;

      return (salary - lowerLimit) * rate;
    }
  }
}
