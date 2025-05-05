using System.ComponentModel.DataAnnotations;

namespace back_end.Models
{
  public class AssignedBenefitListModel
  {
    public Guid? userId { get; set; }
    public Guid? benefitId { get; set; }
    public decimal? validMonths { get; set; }
    public string? userNickname { get; set; }
    public string? benefitName { get; set; }
    public decimal? benefitMinTime { get; set; }
    public string? benefitDescription { get; set; }
    public string? benefitElegibleEmployees { get; set; }
    public Guid? companyId { get; set; }
    public string? companyName { get; set; }
    public Guid? benefitAudit { get; set; }
    public string? userCreator { get; set; }
    public DateTime? creationDate { get; set; }
     public string? userModifier { get; set; }
    public DateTime? modifiedDate { get; set; }
    public string? formulaType { get; set; }
    public string? urlAPI { get; set; }
    public string? formulaParamUno { get; set; }
    public string? formulaParamDos { get; set; }
    public string? formulaParamTres { get; set; }
    public bool isAssigned { get; set; }
  }
}