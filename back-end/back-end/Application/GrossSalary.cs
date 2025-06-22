using System.Collections.Generic;
using back_end.Domain;
using back_end.Infraestructure;

namespace back_end.Application
{
    public class GrossSalary : IGrossSalary
    {
        private const string BIWEEKLY_PAYMENT_TYPE = "quincenal";
        private const string MONTHLY_PAYMENT_TYPE = "mensual";

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
    }
}
