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

    [Required]
    public string username { get; set; }

    [Required]
    public string gender { get; set; }

    [Required]
    public DateTime birthDate { get; set; }

    [Required]
    public int birthDay { get; set; }

    [Required]
    public int birthMonth { get; set; }

    [Required]
    public int birthYear { get; set; }
  }
}