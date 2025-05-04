namespace back_end.Models
{
    public class MyCompanyModel
    {
        public MyCompanyModel()
        {
            this.Name = "No especificado";
            this.Description = "No especificado";
            this.Email = "No especificado";
            this.Phone = "No especificado";
            this.Document = "No especificado";
            this.PaymentType = "No especificado";
            this.Benefits = "No especificado";
            this.Birth = "No especificado";
            this.Province = "No especificado";
            this.Canton = "No especificado";
            this.District = "No especificado";
            this.Address = "No especificado";
            this.Owner = "No especificado";
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Document {  get; set; }
        public string PaymentType { get; set; }
        public string Benefits { get; set; }
        public string Birth {  get; set; }
        public string Province { get; set; }
        public string Canton { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string Owner { get; set; }
    }
}
