using System.ComponentModel.DataAnnotations;

namespace back_end.Models
{
  public class EmployeeBenefitDTO : IBenefitWrapper
  {
    [Required]
    public BenefitDTO benefit { get; set; }

    [Required]
    public bool assigned { get; set; }
  }

  public class AssignBenefitReq
  {
    [Required]
    public string id {get; set; }
  }
}