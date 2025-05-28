using System.ComponentModel.DataAnnotations;

namespace back_end.Models
{
  public class BenefitDTO
  {
    public Guid id { get; set; }
    [Required]
    public string name { get; set; }

    [Required]
    public string description { get; set; }

    [Required]
    public string elegibleEmployees { get; set; }

    [Required]
    public decimal minEmployeeTime { get; set; }

    [Required]
    public string deductionType { get; set; }

    public DateTime creationDate { get; set; }

    public short benefitsPerEmployee {  get; set; }

    public string? userCreator { get; set; }

    public string? userModifier { get; set; }

    public DateTime? modifiedDate { get; set; }

    public string? urlAPI { get; set; }

    public string? paramOneAPI { get; set; }

    public string? paramTwoAPI { get; set; }

    public string? paramThreeAPI { get; set; }
  }
}