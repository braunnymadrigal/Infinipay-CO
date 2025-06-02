namespace back_end.Domain
{
    public class PayrollPreviousComputedGrossSalary
    {
        public required double amount { get; set; }
        public required DateOnly startDate { get; set; }
    }
}
