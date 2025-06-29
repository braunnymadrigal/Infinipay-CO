using System.ComponentModel.DataAnnotations;

namespace back_end.Models
{
  public class EmailModel
  {
    public List<string> recipients { get; set; } = [];
    [Required]
    public string subject { get; set; } = string.Empty;
    [Required]
    public string message { get; set; } = string.Empty;
    public List<IFormFile> attachments { get; set; } = [];
  }
}