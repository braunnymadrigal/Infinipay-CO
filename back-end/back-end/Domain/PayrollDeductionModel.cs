namespace back_end.Domain
{
    public class PayrollDeductionModel
    {
        public required string id { get; set; }
        public required int dependantNumber { get; set; }
        public required string formulaType { get; set; }
        public required string apiUrl { get; set; }
        public required string apiMethod { get; set; }
        public required string param1Value { get; set; }
        public required string param2Value { get; set; }
        public required string param3Value { get; set; }
        public required string param1Key { get; set; }
        public required string param2Key { get; set; }
        public required string param3Key { get; set; }
        public required string header1Value { get; set; }
        public required string header1Key { get; set; }
        public required double resultAmount { get; set; }
    }
}
