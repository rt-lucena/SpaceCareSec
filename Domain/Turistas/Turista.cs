using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceCare.Domain.Turistas
{
    /// <summary>
    /// Representa a entidade de domínio de um Turista Espacial no sistema SpaceCare, contendo informações pessoais, de contato e histórico médico.
    /// </summary>
    [Table("SC_TURISTAS")]
    public class Turista
    {
        /// <summary>
        /// Identificador único do turista, gerado automaticamente pelo banco de dados.
        /// </summary>
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Define o nome completo do turista.
        /// </summary>
        /// <value>O nome com limite máximo de 100 caracteres.</value>
        [Required]
        [StringLength(100)]
        [Column("NOME")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Define o número do passaporte espacial do turista.
        /// </summary>
        /// <value>O valor alfanumérico do passaporte com limite máximo de 20 caracteres.</value>
        [Required]
        [StringLength(20)]
        [Column("NR_PASSAPORTE_ESPACIAL")]
        public string PassaporteEspacial { get; set; } = string.Empty;

        /// <summary>
        /// Define a data de nascimento do turista, utilizada para calcular a idade.
        /// </summary>
        /// <value>A data de nascimento, sendo um campo obrigatório com formato de data válido.</value>
        [Required]
        [Column("DT_NASCIMENTO")]
        public DateTime DataNascimento { get; set; }

        /// <summary>
        /// Define o endereço de e-mail do turista, utilizado para contato.
        /// </summary>
        /// <value>O endereço de e-mail com limite máximo de 150 caracteres.</value>
        [Required]
        [StringLength(150)]
        [Column("EMAIL")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Define observações clínicas, alergias ou condições crônicas.
        /// </summary>
        /// <value>Texto descritivo opcional com limite de 250 caracteres.</value>
        [StringLength(250)]
        [Column("HISTORICO_MEDICO")]
        public string? HistoricoMedico { get; set; }

        /// <summary>
        /// Define a data e hora exata em que o turista foi registrado
        /// </summary>
        [Required]
        [Column("DT_CADASTRO")]
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Define o status de atividade do turista.
        /// O valor "0" representa uma deleção lógica.
        /// </summary>
        /// O caractere indicativo de estado, onde "1" é Ativo e "0" é Inativo.
        [Required]
        [Column("ATIVO")]
        public string Ativo { get; set; } = "1";
    }
}
