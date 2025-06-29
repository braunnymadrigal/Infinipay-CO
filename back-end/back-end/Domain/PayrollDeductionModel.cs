namespace back_end.Domain
{
    public class PayrollDeductionModel
    {
        public PayrollDeductionModel()
        {
            id = string.Empty;
            name = string.Empty;
            formulaType = string.Empty;
            apiUrl = string.Empty;
            apiMethod = string.Empty;
            param1Value = string.Empty;
            param2Value = string.Empty;
            param3Value = string.Empty;
            param1Key = string.Empty;
            param2Key = string.Empty;
            param3Key = string.Empty;
            header1Value = string.Empty;
            header1Key = string.Empty;
        }

        public string id { get; set; }
        public string name { get; set; }
        public int dependantNumber { get; set; }
        public string formulaType { get; set; }
        public string apiUrl { get; set; }
        public string apiMethod { get; set; }
        public string param1Value { get; set; }
        public string param2Value { get; set; }
        public string param3Value { get; set; }
        public string param1Key { get; set; }
        public string param2Key { get; set; }
        public string param3Key { get; set; }
        public string header1Value { get; set; }
        public string header1Key { get; set; }
        public double resultAmount { get; set; }
    }
}
