using back_end.Domain;

namespace back_end.Application
{
    public class MandatoryTaxes : IMandatoryTaxes
    {
        private const double EMPLOYEE_CCSS_SEM = 0.055;
        private const double EMPLOYEE_CCSS_IVM = 0.0417;
        private const double EMPLOYEE_LPT_BPOP = 0.01;

        private const double EMPLOYER_CCSS_SEM = 0.0925;
        private const double EMPLOYER_CCSS_IVM = 0.0542;
        private const double EMPLOYER_OTHERS_BPOP = 0.0025;
        private const double EMPLOYER_OTHERS_FAMILY = 0.05;
        private const double EMPLOYER_OTHERS_IMAS = 0.005;
        private const double EMPLOYER_OTHERS_INA = 0.015;
        private const double EMPLOYER_LPT_BPOP = 0.0025;
        private const double EMPLOYER_LPT_FCL = 0.015;
        private const double EMPLOYER_LPT_OPC = 0.02;
        private const double EMPLOYER_LPT_INS = 0.01;

        private const string HIRING_TYPE_EXCLUDED_FROM_MANDATORY_TAXES = "servicios";

        public List<PayrollEmployeeModel> calculateMandatoryTaxes(List<PayrollEmployeeModel> payrollEmployees)
        {
            for (var i = 0; i < payrollEmployees.Count; ++i)
            {
                if (payrollEmployees[i].hiringType != HIRING_TYPE_EXCLUDED_FROM_MANDATORY_TAXES)
                {
                    payrollEmployees[i] = calculateMandatoryTaxesForEmployee(payrollEmployees[i]);
                    payrollEmployees[i] = calculateMandatoryTaxesForEmployer(payrollEmployees[i]);
                }
            }
            return payrollEmployees;
        }

        private PayrollEmployeeModel calculateMandatoryTaxesForEmployee(PayrollEmployeeModel employee)
        {
            employee.taxes.employeeCcssSem = employee.computedGrossSalary * EMPLOYEE_CCSS_SEM;
            employee.taxes.employeeCcssIvm = employee.computedGrossSalary * EMPLOYEE_CCSS_IVM;
            employee.taxes.employeeLptBpop = employee.computedGrossSalary * EMPLOYEE_LPT_BPOP;
            return employee;
        }

        private PayrollEmployeeModel calculateMandatoryTaxesForEmployer(PayrollEmployeeModel employee)
        {
            employee.taxes.employerCcssSem = employee.computedGrossSalary * EMPLOYER_CCSS_SEM;
            employee.taxes.employerCcssIvm = employee.computedGrossSalary * EMPLOYER_CCSS_IVM;
            employee.taxes.employerOthersBpop = employee.computedGrossSalary * EMPLOYER_OTHERS_BPOP;
            employee.taxes.employerOthersFamily = employee.computedGrossSalary * EMPLOYER_OTHERS_FAMILY;
            employee.taxes.employerOthersImas = employee.computedGrossSalary * EMPLOYER_OTHERS_IMAS;
            employee.taxes.employerOthersIna = employee.computedGrossSalary * EMPLOYER_OTHERS_INA;
            employee.taxes.employerLptBpop = employee.computedGrossSalary * EMPLOYER_LPT_BPOP;
            employee.taxes.employerLptFcl = employee.computedGrossSalary * EMPLOYER_LPT_FCL;
            employee.taxes.employerLptOpc = employee.computedGrossSalary * EMPLOYER_LPT_OPC;
            employee.taxes.employerLptIns = employee.computedGrossSalary * EMPLOYER_LPT_INS;
            return employee;
        }
    }
}
