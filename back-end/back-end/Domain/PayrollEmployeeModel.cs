namespace back_end.Domain
{
    public class PayrollEmployeeModel
    {
        public required string id { get; set; }
        public required string gender { get; set; }
        public required string fullName { get; set; }
        public required DateOnly birthDate { get; set; }
        public required double rentTax { get; set; }
        public required double rawGrossSalary { get; set; }
        public required double rentTax { get; set; }
        public required double computedGrossSalary { get; set; }
        public required double ccssEmployeeDeduction { get; set; }
        public required double ccssEmployerDeduction { get; set; }
        public required DateOnly hiringDate { get; set; }
        public required string hiringType { get; set; }
        public required DateOnly hoursDate { get; set; }
        public required int hoursNumber { get; set; }
        public required string companyAssociation { get; set; }
        public required List<PayrollDeductionModel> deductions { get; set; }
        public required List<PayrollPreviousComputedGrossSalary> previousComputedGrossSalaries { get; set; }
    }
}
