using SpaceCare.Domain.Comportamentos.Enums;
using SpaceCare.Domain.Turistas;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceCare.Domain.Comportamentos
{
    [Table("SC_COMPORTAMENTOS")]
    public class Comportamento
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Required]
        [Column("ID_TURISTA")]
        public int TuristaId { get; set; }

        [Required]
        [Column("DURACAO_SEGUNDOS")]
        public int DuracaoGestoSegundos { get; set; }

        [Required]
        [Column("GESTO")]
        public GestoDetectado GestoDetectado { get; set; }

        [Required]
        [Column("ALERTA")]
        public NivelAlerta NivelAlerta { get; set; }

        [Required]
        [Column("DT_LEITURA")]
        public DateTime DataLeitura { get; set; } = DateTime.UtcNow;

        [ForeignKey("TuristaId")]
        public virtual Turista? Turista { get; set; }
    }
}
