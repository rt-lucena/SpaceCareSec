using System.ComponentModel.DataAnnotations;

namespace SpaceCare.Domain.Turistas.Dtos
{
    /// <summary>
    /// DTO utilizado para receber e validar as informações de cadastro de um novo Turista Espacial.
    /// </summary>
    public class CadastrarTurista
    {
        /// <summary>
        /// Define o nome completo do turista a ser cadastrado.
        /// </summary>
        /// <value>O nome do turista, sendo um campo obrigatório com limite máximo de 100 caracteres.</value>
        [Required(ErrorMessage = "O nome do turista é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode passar de {1} caracteres.")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Define o número do passaporte espacial do turista a ser cadastrado.
        /// </summary>
        /// <value>O valor alfanumérico do passaporte com limite máximo de 20 caracteres.</value>
        [Required(ErrorMessage = "O número do passaporte espacial é obrigatório.")]
        [StringLength(20, ErrorMessage = "O passaporte espacial não pode passar de {1} caracteres.")]
        public string PassaporteEspacial { get; set; } = string.Empty;

        /// <summary>
        /// Define a data de nascimento do turista a ser cadastrado, utilizada para calcular a idade.
        /// </summary>
        /// <value>A data de nascimento, sendo um campo obrigatório com formato de data válido.</value>
        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [DataType(DataType.Date, ErrorMessage = "Formato de data inválido.")]
        public DateTime DataNascimento { get; set; }

        /// <summary>
        /// Define o endereço de e-mail do turista a ser cadastrado, utilizado para contato.
        /// </summary>
        /// <value>O endereço de e-mail com limite máximo de 150 caracteres.</value>
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O formato do e-mail digitado é inválido.")]
        [StringLength(150, ErrorMessage = "O e-mail não pode passar de {1} caracteres.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Define observações clínicas, alergias ou condições crônicas do turista a ser cadastrado.
        /// </summary>
        /// <value>Texto descritivo opcional com limite de 250 caracteres.</value>
        [StringLength(250, ErrorMessage = "O histórico médico pode ter no máximo {1} caracteres.")]
        public string? HistoricoMedico { get; set; }
    }
}
