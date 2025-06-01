using System.ComponentModel.DataAnnotations;

namespace back_end.Domain
{
  public class HoursModel
  {
    [Required]
    public DateOnly date { get; set; }

    [Required]
    public short hoursWorked { get; set; }

    public bool? approved { get; set; }

    public Guid? supervidorId { get; set; }

  }
}
