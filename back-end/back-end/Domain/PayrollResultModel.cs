namespace back_end.Domain
{
  public class EmployeePayrollResult
  {
    public Guid EmployeeId { get; set; }
    public string FullName { get; set; }
    public DateOnly HiringDate { get; set; }
    public string HiringType { get; set; }
    public double ComputedGrossSalary { get; set; }
    public double RentTax { get; set; }
    public double CcssTax { get; set; }
    public List<PayrollDeductionModel> Deductions { get; set; }
    public double TotalDeductions { get; set; }
    public double NetSalary { get; set; }
  }
  public class DeductionResult
  {
    public decimal deductionAmount { get; set; }
    public string deductionType { get; set; }
  }

  public class EmployeeResult
  {
    public string employeeName { get; set; }
    public decimal employeeGrossSalary { get; set; }
    public decimal employeeNetSalary { get; set; }
    public List<DeductionResult> employeeDeductions { get; set; } = new();

    private HashSet<string> employeeUniqueDeductions = new();

    public void addDeduction(DeductionResult deduction)
    {
      string clave = $"{deduction.deductionType}:{deduction.deductionAmount}";

      if (!employeeUniqueDeductions.Contains(clave))
      {
        employeeUniqueDeductions.Add(clave);
        employeeDeductions.Add(deduction);
      }
    }
  }

  public class PayrollResult
  {
    public Guid payrollId { get; set; }
    public DateTime payrollStartDate { get; set; }
    public DateTime payrollEndDate { get; set; }
    public string payrollStatus { get; set; }
    public List<EmployeeResult> payrollEmployees { get; set; } = new();
  }

}
