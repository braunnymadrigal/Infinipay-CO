using System.ComponentModel.DataAnnotations;
using back_end.Application;

namespace back_end.Domain
{
  public class PayrollAdministratorModel
  {
    [Required]
    public string companyName {  get; set; }

    [Required]
    public string hiringType { get; set; }

    [Required]
    public DateOnly startDate { get; set; }

    [Required]
    public DateOnly endDate { get; set; }

    [Required]
    public DateOnly payDate { get; set; }

    [Required]
    public double grossSalary { get; set; }

    [Required]
    public double grossEmployerTax { get; set; }

    [Required]
    public double voluntaryDeductionsTotal { get; set; }

    [Required]
    public double totalEmployerCost { get; set; }
  }
}
