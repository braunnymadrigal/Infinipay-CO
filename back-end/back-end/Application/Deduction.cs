using back_end.Domain;

namespace back_end.Application
{
    public class Deduction : IDeduction
    {
        private const string DEDUCTION_BY_API = "api";
        private const string DEDUCTION_BY_FIXED_AMOUNT = "montoFijo";
        private const string DEDUCTION_BY_PERCENTAGE = "porcentaje";

        private const double PERCENTAGE_DIVISOR = 100.0;

        public List<PayrollEmployeeModel> computeDeductions(List<PayrollEmployeeModel> payrollEmployees)
        {
            for (int i = 0; i < payrollEmployees.Count; ++i)
            {
                var payrollEmployee = payrollEmployees[i];
                var deductions = payrollEmployee.deductions;
                for (int j = 0; j < deductions.Count; ++j)
                {
                    var deduction = deductions[j];
                    switch (deduction.formulaType)
                    {
                        case DEDUCTION_BY_API:
                            //
                            break;
                        case DEDUCTION_BY_FIXED_AMOUNT:
                            payrollEmployees[i].deductions[j] = deductionByFixedAmount(deduction);
                            break;
                        case DEDUCTION_BY_PERCENTAGE:
                            payrollEmployees[i].deductions[j] = deductionByPercentage(deduction, payrollEmployees[i].computedGrossSalary);
                            break;
                        default:
                            throw new Exception("A type of deduction is not supported.");
                    }
                }
            }
            return payrollEmployees;
        }

        private PayrollDeductionModel deductionByFixedAmount(PayrollDeductionModel deduction)
        {
            deduction.resultAmount = convertStringToDouble(deduction.param1Value);
            return deduction;
        }

        private PayrollDeductionModel deductionByPercentage(PayrollDeductionModel deduction, double salary)
        {
            var percentage = convertStringToDouble(deduction.param1Value);
            percentage = percentage / PERCENTAGE_DIVISOR;
            deduction.resultAmount = salary * percentage;
            return deduction;
        }

        private double convertStringToDouble(string value)
        {
            var convertedValue = 0.0;
            if (!double.TryParse(value, out convertedValue) 
                || Double.IsNaN(convertedValue) || Double.IsInfinity(convertedValue) 
                || Double.IsNegative(convertedValue))
            {
                throw new Exception("The string does not represent a positive proper double number.");
            }
            return convertedValue;
        }
    }
}
