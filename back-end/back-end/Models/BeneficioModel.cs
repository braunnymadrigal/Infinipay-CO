/*using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Models
{
    public class BeneficioModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [Column(TypeName = "decimal(4,2)")]
        public decimal TiempoMinimo { get; set; }

        [Required]
        [MaxLength(256)]
        public string Descripcion { get; set; }

        [Required]
        [MaxLength(9)]
        [RegularExpression("^(todos|semanal|quincenal|mensual)$")]
        public string EmpleadoElegible { get; set; }

        [Required]
        public Guid IdPersonaJuridica { get; set; }

        [ForeignKey("IdPersonaJuridica")]
        public PersonaJuridica PersonaJuridica { get; set; }

        [Required]
        public Guid IdAuditoria { get; set; }

        [ForeignKey("IdAuditoria")]
        public Auditoria Auditoria { get; set; }
    }
}

*/