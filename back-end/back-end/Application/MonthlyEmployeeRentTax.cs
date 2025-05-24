namespace back_end.Application
{
  public class MonthlyEmployeeRentTax : IRentTax
  {
    public decimal CalculateRentTax(decimal salary)
    {
      decimal tax = 0;

      switch (salary)
      {
        case <= 922_000:
          tax = 0;
          break;

        case <= 1_352_000:
          tax = (salary - 922_000m) * 0.10m;
          break;

        case <= 2_373_000:
          tax = (1_352_000m - 922_000m) * 0.10m +
                (salary - 1_352_000m) * 0.15m;
          break;

        case <= 4_745_000:
          tax = (1_352_000m - 922_000m) * 0.10m +
                (2_373_000m - 1_352_000m) * 0.15m +
                (salary - 2_373_000m) * 0.20m;
          break;

        default:
          tax = (1_352_000m - 922_000m) * 0.10m +
                (2_373_000m - 1_352_000m) * 0.15m +
                (4_745_000m - 2_373_000m) * 0.20m +
                (salary - 4_745_000m) * 0.25m;
          break;
      }

      return Math.Round(tax, 2);
    }
  }
}
