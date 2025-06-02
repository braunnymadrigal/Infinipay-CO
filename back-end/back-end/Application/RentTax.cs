using System;
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

    public List<RentTaxModel> calculateRentTaxes(List<GrossSalaryModel>
      grossSalaries)
    {
      var rentTaxes = new List<RentTaxModel>();

      foreach (var s in grossSalaries)
      {
        var tax = 0.0;

        if (s.HiringType?.ToLower() != HIRING_TYPE_EXCLUDED_FROM_RENT_TAX)
        {
          tax = calculateRentTax(s.ComputedGrossSalary);
        }

        rentTaxes.Add(new RentTaxModel
        {
          rentTax = tax
        });
      }

      return rentTaxes;
    }

    public double calculateRentTax(double grossSalary)
    {
      if (grossSalary <= TIER_1_LIMIT)
        return 0;

      double totalTax = 0;

      totalTax += CalculateTierTax(grossSalary, TIER_1_LIMIT, TIER_2_LIMIT
        , TIER_2_RATE);
      totalTax += CalculateTierTax(grossSalary, TIER_2_LIMIT, TIER_3_LIMIT
        , TIER_3_RATE);
      totalTax += CalculateTierTax(grossSalary, TIER_3_LIMIT, TIER_4_LIMIT
        , TIER_4_RATE);
      totalTax += CalculateExcessTax(grossSalary, TIER_4_LIMIT, TIER_5_RATE);

      return totalTax;
    }

    private double CalculateTierTax(double salary, double lowerLimit
      , double upperLimit, double rate)
    {
      if (salary <= lowerLimit)
        return 0;

      double taxableAmount = Math.Min(salary, upperLimit) - lowerLimit;
      return taxableAmount * rate;
    }

    private double CalculateExcessTax(double salary, double lowerLimit
      , double rate)
    {
      if (salary <= lowerLimit)
        return 0;

      return (salary - lowerLimit) * rate;
    }
  }
}