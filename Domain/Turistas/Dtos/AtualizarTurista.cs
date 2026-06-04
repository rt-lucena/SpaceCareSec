using System.ComponentModel.DataAnnotations;

namespace SpaceCare.Domain.Turistas.Dtos
{
    /// <summary>
    /// DTO utilizado para receber e validar as informações de atualização de um Turista Espacial existente.
    /// </summary>
    public class AtualizarTurista
    {
        /// <summary>
        /// Define o identificador único do turista a ser atualizado.
        /// </summary>
        /// <value>O ID do turista, sendo um campo obrigatório para realizar a atualização.</value>
        [Required(ErrorMessage = "O ID do turista é obrigatório para realizar a atualização.")]
        public int Id { get; set; }
        
        /// <summary>
        /// Define o nome completo do turista a ser atualizado.
        /// </summary>
        /// <value>O nome atualizado, sendo um campo opcional com limite máximo de 100 caracteres.</value>
        [StringLength(100, ErrorMessage = "O nome não pode passar de {1} caracteres.")]
        public string? Nome { get; set; }

        /// <summary>
        /// Define o número do passaporte espacial do turista a ser atualizado.
        /// </summary>
        /// <value>O número atualizado do passaporte espacial, sendo um campo opcional com limite máximo de 20 caracteres.</value>
        [StringLength(20, ErrorMessage = "O passaporte espacial não pode passar de {1} caracteres.")]
        public string? PassaporteEspacial { get; set; }
        
        /// <summary>
        /// Define o endereço de e-mail do turista a ser atualizado.
        /// </summary>
        /// <value>O e-mail atualizado, sendo um campo opcional com limite máximo de 150 caracteres.</value>
        [EmailAddress(ErrorMessage = "O formato do e-mail digitado é inválido.")]
        [StringLength(150, ErrorMessage = "O e-mail não pode passar de {1} caracteres.")]
        public string? Email { get; set; }

        /// <summary>
        /// Define o histórico médico do turista a ser atualizado.
        /// </summary>
        /// <value>O texto descritivo atualizado, sendo um campo opcional com limite máximo de 250 caracteres.</value>
        [StringLength(250, ErrorMessage = "O histórico médico pode ter no máximo {1} caracteres.")]
        public string? HistoricoMedico { get; set; }
    }
}
