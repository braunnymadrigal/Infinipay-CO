using System.Collections.Generic;
using back_end.Domain;
using back_end.Infraestructure;

namespace back_end.Application
{
    public class GrossSalary : IGrossSalary
    {
        private const string BIWEEKLY_PAYMENT_TYPE = "quincenal";
        private const string MONTHLY_PAYMENT_TYPE = "mensual";

        private const double P_VALUE_ON_DECIMAL_SQL_TYPE = 11.0;
        private const double S_VALUE_ON_DECIMAL_SQL_TYPE = 2.0;
        private const double EXPONENTATION_BASE_VALUE_ON_DECIMAL_SQL_TYPE = 10.0;

        private IContextGrossSalaryComputation _contextGrossSalaryComputation;

        public GrossSalary(IContextGrossSalaryComputation contextGrossSalaryComputation)
        {
            _contextGrossSalaryComputation = contextGrossSalaryComputation;
        }

        public List<PayrollEmployeeModel> computeAllGrossSalaries(List<PayrollEmployeeModel> 
            payrollEmployees, PayrollEmployerModel payrollEmployer)
        {
            setGrossSalaryComputationStrategy(payrollEmployer.paymentType);
            payrollEmployees = _contextGrossSalaryComputation.computeGrossSalary(payrollEmployees, 
                payrollEmployer.startDate, payrollEmployer.endDate);
            validateComputedGrossSalaries(payrollEmployees);
            return payrollEmployees;
        }

        private void setGrossSalaryComputationStrategy(string paymentType)
        {
            switch (paymentType)
            {
                case BIWEEKLY_PAYMENT_TYPE:
                    _contextGrossSalaryComputation.setStrategy(new BiweeklyGrossSalaryComputation());
                    break;
                case MONTHLY_PAYMENT_TYPE:
                    _contextGrossSalaryComputation.setStrategy(new MonthlyGrossSalaryComputation());
                    break;
                default:
                    throw new Exception("GrossSalary: Improper strategy have been specified.");
            }
        }

        private void validateComputedGrossSalaries(List<PayrollEmployeeModel> payrollEmployees)
        {
            foreach (var payrollEmployee in payrollEmployees)
            {
                validateComputedGrossSalaryGreaterThanZero(payrollEmployee.computedGrossSalary);
                validateComputedGrossSalarySize(payrollEmployee.computedGrossSalary);
            }
        }

        private void validateComputedGrossSalarySize(double computedGrossSalary)
        {
            var maxSize =
                (Math.Pow(
                    EXPONENTATION_BASE_VALUE_ON_DECIMAL_SQL_TYPE,
                    (P_VALUE_ON_DECIMAL_SQL_TYPE - S_VALUE_ON_DECIMAL_SQL_TYPE)
                    )
                ) -
                (Math.Pow(EXPONENTATION_BASE_VALUE_ON_DECIMAL_SQL_TYPE
                , -S_VALUE_ON_DECIMAL_SQL_TYPE));
            if (computedGrossSalary > maxSize)
            {
                throw new Exception("The computed gross salary can not exceed" +
                  "the database limitations");
            }
        }

        private void validateComputedGrossSalaryGreaterThanZero(double
          computedGrossSalary)
        {
            if (computedGrossSalary < 0)
            {
                throw new Exception("The computed gross salary can not be" +
                  "less than zero.");
            }
        }
    }
}
