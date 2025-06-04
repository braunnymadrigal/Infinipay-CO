using System.Diagnostics;
using back_end.Domain;

namespace back_end.Application
{
  public class PayrollOrchestrator
  {
    private readonly IPayrollEmployee payrollEmployee;
    private readonly IGrossSalary grossSalary;
    private readonly IRentTax rentTax;
    private readonly ITaxCCSS taxCCSS;
    private readonly IDeduction deduction;

    public PayrollOrchestrator(
        IPayrollEmployee payrollEmployee,
        IGrossSalary grossSalary,
        IRentTax rentTax,
        ITaxCCSS taxCCSS,
        IDeduction deduction)
    {
      this.payrollEmployee = payrollEmployee;
      this.grossSalary = grossSalary;
      this.rentTax = rentTax;
      this.taxCCSS = taxCCSS;
      this.deduction = deduction;
    }

    public async Task<List<EmployeePayrollResult>>
      ComputePayrollAsync(string employerId, DateOnly start, DateOnly end)
    {
      var payrollEmployees = payrollEmployee.getPayrollEmployees(employerId
        , start, end);
      var grossSalaries = grossSalary.computeAllGrossSalaries(payrollEmployees
        , start, end);
      var rentTaxes = rentTax.calculateRentTaxes(payrollEmployees
        , end);
      var ccssTaxes = taxCCSS.computeTaxesCCSS(payrollEmployees, end);
      var deductions = await deduction.computeDeductions(payrollEmployees);

      var employeePayroll = grossSalaries.Select((s, i) =>
      {
        var rentTaxModel = rentTaxes[i];
        var ccssTaxModel = ccssTaxes[i];
        var deductionModel = deductions[i];
        double netSalary = s.computedGrossSalary;
        double totalAppliedDeductions = 0;

        double appliedRentTax = 0;
        double appliedCcssTax = 0;

        List<PayrollDeductionModel> appliedBenefitDeductions = new();
        List<PayrollDeductionModel> allDeductions = deductionModel.deductions;

        if (netSalary - rentTaxModel.rentTax > 0)
        {
          appliedRentTax = rentTaxModel.rentTax;
          netSalary -= appliedRentTax;
          totalAppliedDeductions += appliedRentTax;
        }

        if (netSalary - ccssTaxModel.ccssEmployeeDeduction > 0)
        {
          appliedCcssTax = ccssTaxModel.ccssEmployeeDeduction;
          netSalary -= appliedCcssTax;
          totalAppliedDeductions += appliedCcssTax;
        }

        foreach (var deductions in allDeductions)
        {
          if (netSalary - deductions.resultAmount > 0)
          {
            appliedBenefitDeductions.Add(deductions);
            netSalary -= deductions.resultAmount;
            totalAppliedDeductions += deductions.resultAmount;
          }
        }
        var guidOnly = s.id.Split(' ')[0];
        var parsedId = Guid.Parse(guidOnly);
        return new EmployeePayrollResult
        {
          FullName = s.fullName,
          HiringDate = s.hiringDate,
          HiringType = s.hiringType,
          EmployeeId = parsedId,
          ComputedGrossSalary = s.computedGrossSalary,
          RentTax = appliedRentTax,
          CcssTax = appliedCcssTax,
          Deductions = appliedBenefitDeductions,
          TotalDeductions = totalAppliedDeductions,
          NetSalary = netSalary
        };
      }).ToList();

      return employeePayroll;
    }
  }
}
