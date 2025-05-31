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
      double calculatedTax;

      switch (grossSalary)
      {
        case <= TIER_1_LIMIT:
          calculatedTax = 0;
          break;

        case <= TIER_2_LIMIT:
          calculatedTax = (grossSalary - TIER_1_LIMIT) * TIER_2_RATE;
          break;

        case <= TIER_3_LIMIT:
          calculatedTax = (TIER_2_LIMIT - TIER_1_LIMIT) * TIER_2_RATE +
                          (grossSalary - TIER_2_LIMIT) * TIER_3_RATE;
          break;

        case <= TIER_4_LIMIT:
          calculatedTax = (TIER_2_LIMIT - TIER_1_LIMIT) * TIER_2_RATE +
                          (TIER_3_LIMIT - TIER_2_LIMIT) * TIER_3_RATE +
                          (grossSalary - TIER_3_LIMIT) * TIER_4_RATE;
          break;

        default:
          calculatedTax = (TIER_2_LIMIT - TIER_1_LIMIT) * TIER_2_RATE +
                          (TIER_3_LIMIT - TIER_2_LIMIT) * TIER_3_RATE +
                          (TIER_4_LIMIT - TIER_3_LIMIT) * TIER_4_RATE +
                          (grossSalary - TIER_4_LIMIT) * TIER_5_RATE;
          break;
      }

      return calculatedTax;
    }
  }
}