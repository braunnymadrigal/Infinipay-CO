using back_end.Domain;

namespace back_end.Application
{
  public class PayrollOrchestrator : IPayrollOrchestrator
  {
    private readonly IPayrollEmployer payrollEmployer;
    private readonly IPayrollEmployee payrollEmployee;
    private readonly IGrossSalary grossSalary;
    private readonly IRentTax rentTax;
    private readonly IMandatoryTaxes mandatoryTaxes;
    private readonly IDeduction deduction;

    public PayrollOrchestrator(PayrollEmployee payrollEmployee
      , GrossSalary grossSalary, RentTax rentTax
      , MandatoryTaxes mandatoryTaxes, Deduction deduction)
    {
      this.payrollEmployee = payrollEmployee;
      this.grossSalary = grossSalary;
      this.rentTax = rentTax;
      this.mandatoryTaxes = mandatoryTaxes;
      this.deduction = deduction;
    }

    public PayrollOrchestrator(
        IPayrollEmployer payrollEmployer,
        IPayrollEmployee payrollEmployee,
        IGrossSalary grossSalary,
        IRentTax rentTax,
        IMandatoryTaxes mandatoryTaxes,
        IDeduction deduction)
    {
      this.payrollEmployer = payrollEmployer;
      this.payrollEmployee = payrollEmployee;
      this.grossSalary = grossSalary;
      this.rentTax = rentTax;
      this.mandatoryTaxes = mandatoryTaxes;
      this.deduction = deduction;
    }

    public async Task<List<EmployeePayrollResult>>
      ComputePayrollAsync(string employerId, DateOnly start, DateOnly end)
    {
      var payrollEmployerData = payrollEmployer.getPayrollEmployer(
        employerId, start, end);
      var payrollEmployees = payrollEmployee.getPayrollEmployees(
        payrollEmployerData);

      var grossSalaries = grossSalary.computeAllGrossSalaries(payrollEmployees
        , payrollEmployerData);
      var rentTaxes = rentTax.calculateRentTaxes(payrollEmployees, end);
      var ccssTaxes = mandatoryTaxes.calculateMandatoryTaxes(payrollEmployees);
      var deductions = await deduction.calculateDeductions(payrollEmployees
        , payrollEmployerData.paymentType);

      var employeePayroll = grossSalaries.Select((s, i) =>
      {
        var rentTaxModel = rentTaxes[i];
        var ccssTaxesModel = ccssTaxes[i];
        var deductionModel = deductions[i];

        double netSalary = s.computedGrossSalary;
        double totalAppliedDeductions = 0;

        List<PayrollDeductionModel> appliedBenefitDeductions = new();
        List<PayrollDeductionModel> allDeductions = deductionModel.deductions;

        var employeeTaxesModel = new PayrollTaxModel();
        var employerTaxesModel = new PayrollTaxModel();

        if (netSalary - rentTaxModel.taxes.employeeRent > 0)
        {
          employeeTaxesModel.employeeRent = rentTaxModel.taxes.employeeRent;
          netSalary -= employeeTaxesModel.employeeRent;
          totalAppliedDeductions += employeeTaxesModel.employeeRent;
        }

        employeeTaxesModel.employeeCcssSem = ccssTaxesModel.taxes.employeeCcssSem;
        employeeTaxesModel.employeeCcssIvm = ccssTaxesModel.taxes.employeeCcssIvm;
        employeeTaxesModel.employeeLptBpop = ccssTaxesModel.taxes.employeeLptBpop;

        var employeeTaxesList = new List<double>
        {
          employeeTaxesModel.employeeCcssSem,
          employeeTaxesModel.employeeCcssIvm,
          employeeTaxesModel.employeeLptBpop
        };

        foreach (var tax in employeeTaxesList)
        {
          if (netSalary - tax > 0)
          {
            netSalary -= tax;
            totalAppliedDeductions += tax;
          }
        }

        foreach (var benefit in allDeductions)
        {
          if (netSalary - benefit.resultAmount > 0)
          {
            appliedBenefitDeductions.Add(benefit);
            netSalary -= benefit.resultAmount;
            totalAppliedDeductions += benefit.resultAmount;
          }
        }

        employerTaxesModel.employerCcssSem = ccssTaxesModel.taxes.employerCcssSem;
        employerTaxesModel.employerCcssIvm = ccssTaxesModel.taxes.employerCcssIvm;
        employerTaxesModel.employerOthersBpop = ccssTaxesModel.taxes.employerOthersBpop;
        employerTaxesModel.employerOthersFamily = ccssTaxesModel.taxes.employerOthersFamily;
        employerTaxesModel.employerOthersImas = ccssTaxesModel.taxes.employerOthersImas;
        employerTaxesModel.employerOthersIna = ccssTaxesModel.taxes.employerOthersIna;
        employerTaxesModel.employerLptBpop = ccssTaxesModel.taxes.employerLptBpop;
        employerTaxesModel.employerLptFcl = ccssTaxesModel.taxes.employerLptFcl;
        employerTaxesModel.employerLptOpc = ccssTaxesModel.taxes.employerLptOpc;
        employerTaxesModel.employerLptIns = ccssTaxesModel.taxes.employerLptIns;

        var guidOnly = s.id.Split(' ')[0];
        var parsedId = Guid.Parse(guidOnly);

        return new EmployeePayrollResult
        {
          FullName = s.name,
          HiringDate = s.hiringDate,
          HiringType = s.hiringType,
          EmployeeId = parsedId,
          ComputedGrossSalary = s.computedGrossSalary,
          EmployeeTaxes = employeeTaxesModel,
          EmployerTaxes = employerTaxesModel,
          Deductions = appliedBenefitDeductions,
          TotalDeductions = totalAppliedDeductions,
          NetSalary = netSalary
        };
      }).ToList();

      return employeePayroll;
    }
  }
}