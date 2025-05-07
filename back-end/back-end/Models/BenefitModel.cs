using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace back_end.Models
{
    public class BenefitModel
    {
        [JsonPropertyName("name")]
        public string? BenefitName { get; set; }

        [JsonPropertyName("description")]
        public string? BenefitDescription { get; set; }

        [JsonPropertyName("elegibleEmployees")]
        public string? BenefitElegibleEmployees { get; set; }

        [JsonPropertyName("minMonths")]
        public decimal? BenefitMinTime { get; set; }

        [JsonPropertyName("typeFormula")]
        public string? FormulaType { get; set; }

        [JsonPropertyName("param1")]
        public string? formulaParamUno { get; set; }
        
        [JsonPropertyName("param2")]
        public string? formulaParamDos { get; set; }

        [JsonPropertyName("param3")]
        public string? formulaParamTres { get; set; }

        [JsonPropertyName("userid")]
        public string? UserCreator { get; set; }

        [JsonPropertyName("apiUrl")]
        public string? urlAPI { get; set; }

        public Guid? BenefitId { get; set; }

        public int? BenefitsPerEmployee { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? UserModifier { get; set; }
        public DateTime? ModifiedDate { get; set; }


    }
}
