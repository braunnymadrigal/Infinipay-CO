using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace back_end.Models
{

  public class EmployerModel
  {
    [Required]
    public int idNumber { get; set; }
    
    [Required]
    public string phoneNumber { get; set; }

    [Required]
    public string email { get; set; }

    [Required]
    public string firstName { get; set; }

    [Required]
    public string secondName { get; set; }

    [Required]
    public string firstLastName { get; set; }

    [Required]
    public string secondLastName { get; set; }

    [Required]
    public string province { get; set; }

    [Required]
    public string canton { get; set; }

    [Required]
    public string district { get; set; }

    public string otherSigns { get; set; }

    [Required]
    public string idType { get; set; }
  }
}