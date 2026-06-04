using SpaceCare.Domain.Turistas;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceCare.Domain.Telemetrias
{
    /// <summary>
    /// Representa a entidade de domínio de uma Telemetria no SpaceCare.
    /// Mapeia as leituras de sinais vitais capturadas por sensores.
    /// </summary>
    [Table("SC_TELEMETRIAS")]
    public class Telemetria
    {
        /// <summary>
        /// Identificador único da telemetria, gerado automaticamente pelo banco de dados.
        /// </summary>
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Define o identificador do turista espacial associado a esta telemetria.
        /// Atua como a chave estrangeira da relação.
        /// </summary>
        [Required]
        [Column("TURISTA_ID")]
        public int TuristaId { get; set; }

        /// <summary>
        /// Define a frequência cardíaca registrada do turista.
        /// </summary>
        /// <value>Valor da frequência em batimentos por minuto (BPM).</value>
        [Required]
        [Column("BATIMENTOS")]
        public int BatimentosCardiacos { get; set; }

        /// <summary>
        /// Define a temperatura corporal do turista.
        /// </summary>
        /// <value>Valor da temperatura em graus Celsius (°C).</value>
        [Required]
        [Column("TEMPERATURA", TypeName = "NUMBER(4,2)")]
        public decimal TemperaturaCorporal { get; set; }

        /// <summary>
        /// Define a pressão arterial aferida do astronauta (ex: "12/8" ou "120/80").
        /// </summary>
        /// <value>Texto descritivo contendo os níveis sistólico e diastólico com limite máximo de 10 caracteres.</value>
        [Required]
        [StringLength(10)]
        [Column("PRESSAO_ARTERIAL")]
        public string PressaoArterial { get; set; } = string.Empty;

        /// <summary>
        /// Define a data e hora exata em que a telemetria foi capturada.
        /// </summary>
        /// <value>Por padrão, assume o horário atual em formato UTC (<see cref="DateTime.UtcNow"/>).</value>
        [Required]
        [Column("DT_LEITURA")]
        public DateTime DataLeitura { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Propriedade de navegação virtual para acesso aos dados consolidados do turista associado.
        /// Mapeia o relacionamento de integridade referencial gerenciado pelo Entity Framework Core.
        /// </summary>
        [ForeignKey("TuristaId")]
        public virtual Turista? Turista { get; set; }
    }
}
