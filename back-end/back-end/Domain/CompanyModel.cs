using System.ComponentModel.DataAnnotations;
namespace back_end.Domain
{
  public class CompanyModel
  {
    [Required]
    public string idNumber { get; set; }
    [Required]
    public string phoneNumber { get; set; }
    [Required]
    public string email { get; set; }
    [Required]
    public string legalName { get; set; }
    [Required]
    public string employerUsername { get; set; }
    public string description { get; set; }
    [Required]
    public string paymentType { get; set; }
    [Required]
    public int benefits { get; set; }
    [Required]
    public string province { get; set; }
    [Required]
    public string canton { get; set; }
    [Required]
    public string district { get; set; }
    public string otherSigns { get; set; }

    [Required]
    public int creationDay { get; set; }
    [Required]
    public int creationMonth { get; set; }
    [Required]
    public int creationYear { get; set; }
  }
}