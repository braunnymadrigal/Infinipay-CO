using System.ComponentModel.DataAnnotations;

namespace back_end.Models
{
  public class CompanyBenefitDTO : IBenefitWrapper
  {
    [Required]
    public BenefitDTO benefit { get; set; }
  }
}
