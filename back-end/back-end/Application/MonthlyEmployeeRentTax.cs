namespace back_end.Application
{
  public class MonthlyEmployeeRentTax : IRentTax
  {
    private const decimal TIER_1_LIMIT = 922_000m;
    private const decimal TIER_2_LIMIT = 1_352_000m;
    private const decimal TIER_3_LIMIT = 2_373_000m;
    private const decimal TIER_4_LIMIT = 4_745_000m;

    private const decimal TIER_2_RATE = 0.10m;
    private const decimal TIER_3_RATE = 0.15m;
    private const decimal TIER_4_RATE = 0.20m;
    private const decimal TIER_5_RATE = 0.25m;

    public decimal CalculateRentTax(decimal grossSalary)
    {
      decimal calculatedTax;

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
      return Math.Round(calculatedTax, 2);
    }
  }
}
