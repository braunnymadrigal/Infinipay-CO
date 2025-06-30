namespace back_end.Domain
{
  public class EmployeeFullReport
  {
    public string fullName { get; set; }
    public string contractType { get; set; }
    public string companyName { get; set; }
    public List<PaymentReport> payments { get; set; } = new();
  }

  public class PaymentReport
  {
    public decimal computedGrossSalary { get; set; }
    public decimal netSalary { get; set; }
    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }
    public List<DeductionDetail> deductions { get; set; } = new();
  }

  public class DeductionDetail
  {
    public string type { get; set; }
    public decimal amount { get; set; }
    public string deductionName { get; set; }
  }

}