using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Models
{
    public class BenefitModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        // [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(4,2)")]
        public decimal MinMonths { get; set; }

        [Required]
        // [MaxLength(256)]
        public string Description { get; set; }

        [Required]
        // [MaxLength(9)]
        // [RegularExpression("^(todos|semanal|quincenal|mensual)$")]
        public string ElegibleEmployee { get; set; }

        [Required]
        public string legalName { get; set; }

        [Required]
        public string deductionType { get; set; }

        [Required]
        public int payment { get; set; }
        // [ForeignKey("IdPersonaJuridica")]
        // public PersonaJuridica PersonaJuridica { get; set; }

        // [Required]
        // public Guid IdAuditoria { get; set; }

        // [ForeignKey("IdAuditoria")]
        // public Auditoria Auditoria { get; set; }
    }
}

