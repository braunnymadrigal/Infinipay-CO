using System.ComponentModel.DataAnnotations;

namespace back_end.Models
{
  public class AssignedBenefitListModel
  {
    public Guid? benefitId { get; set; }
    public string? benefitName { get; set; }
    public decimal? benefitMinTime { get; set; }
    public string? benefitDescription { get; set; }
    public string? benefitElegibleEmployees { get; set; }
    public string? userCreator { get; set; }
    public DateTime? creationDate { get; set; }
     public string? userModifier { get; set; }
    public DateTime? modifiedDate { get; set; }
    public string? formulaType { get; set; }
    public string? urlAPI { get; set; }
    public string? formulaParamUno { get; set; }
    public string? formulaParamDos { get; set; }
    public string? formulaParamTres { get; set; }
    public short? beneficiosPorEmpleado { get; set; }
    public bool? asignado { get; set; }
  }

  public class AssignBenefitRequest
  {
    public string benefitId { get; set; }
  }
}