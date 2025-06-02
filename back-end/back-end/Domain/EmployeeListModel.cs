using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace back_end.Domain
{
  public class EmployeeListModel
  {
    public Guid id { get; set; }
    public string completeName { get; set; }
    public string firstName { get; set; }
    public string secondName { get; set; }
    public string firstLastName { get; set; }
    public string secondLastName { get; set; }
    public string identification { get; set; }
    public string nickname { get; set; }
    public string email { get; set; }
    public string phoneNumber { get; set; }
    public string role { get; set; }
    public string hireDate { get; set; }
    public string supervisor { get; set; }
    public string observations { get; set; }
    public string province { get; set; }
    public string canton { get; set; }
    public string district { get; set; }
    public string otherSigns { get; set; }
  }
}