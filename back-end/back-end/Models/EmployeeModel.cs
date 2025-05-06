using System.ComponentModel.DataAnnotations;

namespace back_end.Models
{
  public class EmployeeModel
  {
    [Required]
    public string idNumber { get; set; }

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
    public string username { get; set; }

    [Required]
    public string gender { get; set; }

    [Required]
    public int birthDay { get; set; }

    [Required]
    public int birthMonth { get; set; }

    [Required]
    public int birthYear { get; set; }

    [Required]
    public int hireDay { get; set; }

    [Required]
    public int hireMonth { get; set; }

    [Required]
    public int hireYear { get; set; }

    [Required]
    public string role { get; set; }

    [Required]
    public int creationDay { get; set; }

    [Required]
    public int creationMonth { get; set; }

    [Required]
    public int creationYear { get; set; }

    [Required]
    public int salary { get; set; }

    [Required]
    public int reportsHours { get; set; }

    [Required]
    public string typeContract { get; set; }
  }
}
