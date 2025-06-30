namespace back_end.Domain
{
  public class EmployerPayrollReport
  {
    public string employerFullName { get; set; }
    public string companyName { get; set; }
    public List<PayPeriodSummary> periodSummaries { get; set; } = new();
  }

  public class PayPeriodSummary
  {
    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }
    public decimal totalEmpHoursSalary { get; set; }
    public decimal totalEmpFullTimeSalary { get; set; }
    public decimal totalEmpHalfTimeSalary { get; set; }
    public decimal totalEmpServicesSalary { get; set; }
    public decimal totalSalaries { get; set; }
    public decimal totalEmployerCcssIvm { get; set; }
    public decimal totalEmployerCcssSem { get; set; }
    public decimal totalEmployerLptBpop { get; set; }
    public decimal totalEmployerLptOpc { get; set; }
    public decimal totalEmployerLptFcl { get; set; }
    public decimal totalEmployerLptIns { get; set; }
    public decimal totalEmployerOtrasIna { get; set; }
    public decimal totalEmployerOtrasImas { get; set; }
    public decimal totalEmployerOtrasFamiliares { get; set; }
    public decimal totalEmployerOtrasBpop { get; set; }

  }

}