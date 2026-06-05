using SpaceCare.Domain.Comportamentos.Enums;
using SpaceCare.Domain.Turistas;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceCare.Domain.Comportamentos
{
    /// <summary>
    /// Representa a entidade de domínio de um Comportamento detectado no SpaceCare.
    /// Mapeia os registros de gestos monitorados por visão computacional.
    /// </summary>
    [Table("SC_COMPORTAMENTOS")]
    public class Comportamento
    {
        /// <summary>
        /// Identificador único do comportamento, gerado automaticamente pelo banco de dados.
        /// </summary>
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Define o identificador do turista espacial associado a este comportamento.
        /// Atua como a chave estrangeira da relação.
        /// </summary>
        [Required]
        [Column("ID_TURISTA")]
        public int TuristaId { get; set; }

        /// <summary>
        /// Define a duração total do gesto detectado.
        /// </summary>
        /// <value>A duração medida estritamente em segundos.</value>
        [Required]
        [Column("DURACAO_SEGUNDOS")]
        public int DuracaoGestoSegundos { get; set; }

        /// <summary>
        /// Define o tipo de gesto mapeado pelo sistema de monitoramento inteligente.
        /// </summary>
        /// <value>O valor do enumerador <see cref="GestoDetectado"/> correspondente à ação realizada.</value>
        [Required]
        [Column("GESTO")]
        public GestoDetectado GestoDetectado { get; set; }

        /// <summary>
        /// Define o nível de criticidade ou gravidade determinado para o comportamento apresentado.
        /// </summary>
        /// <value>O valor do enumerador <see cref="NivelAlerta"/> utilizado para triagem de urgência médica.</value>
        [Required]
        [Column("ALERTA")]
        public NivelAlerta NivelAlerta { get; set; }

        /// <summary>
        /// Define a data e hora exata em que o comportamento foi detectado.
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
