namespace back_end.Domain
{
    public class GrossSalaryModel
    {
        public required string EmployeeId { get; set; }
        public required DateOnly HiringDate { get; set; }
        public required double ComputedGrossSalary { get; set; }
        public required double GrossSalary { get; set; }
        public required string HiringType { get; set; }
        public required DateOnly HoursDate { get; set; }
        public required int HoursWorked { get; set; }
    }
}
