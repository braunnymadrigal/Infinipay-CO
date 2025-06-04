using System.ComponentModel.DataAnnotations;

namespace back_end.Domain
{

  public class EmployeeHoursModel
  {
    [Required]
    public string typeContract { get; set; }

    [Required]
    public bool reportsHours { get; set; }
  }
}