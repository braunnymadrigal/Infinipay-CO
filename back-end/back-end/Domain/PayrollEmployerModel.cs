namespace back_end.Domain
{
    public class PayrollEmployerModel
    {
        public PayrollEmployerModel()
        {
            id = string.Empty;
            paymentType = string.Empty;
        }

        public string id { get; set; }
        public string paymentType { get; set; }
        public DateOnly startDate { get; set; }
        public DateOnly endDate { get; set; }
        public DateOnly latestEndDate { get; set; }
    }
}
