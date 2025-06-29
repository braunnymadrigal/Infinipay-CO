namespace back_end.Domain
{
    public class PayrollEmployeeModel
    {
        public PayrollEmployeeModel()
        {
            id = string.Empty;
            name = string.Empty;
            gender = string.Empty;
            hiringType = string.Empty;
            companyAssociation = string.Empty;
            taxes = new PayrollTaxModel();
            deductions = new List<PayrollDeductionModel>();
            previousComputedGrossSalaries = new List<double>();
        }

        public string id { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public DateOnly birthDate { get; set; }
        public DateOnly hiringDate { get; set; }
        public string hiringType { get; set; }
        public string companyAssociation { get; set; }
        public double rawGrossSalary { get; set; }
        public double computedGrossSalary { get; set; }
        public PayrollTaxModel taxes { get; set; }
        public List<PayrollDeductionModel> deductions { get; set; }
        public List<double> previousComputedGrossSalaries { get; set; }
    }
}
