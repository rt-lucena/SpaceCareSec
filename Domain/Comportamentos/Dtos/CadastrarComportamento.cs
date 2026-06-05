using SpaceCare.Domain.Comportamentos.Enums;
using System.ComponentModel.DataAnnotations;

namespace SpaceCare.Domain.Comportamentos.Dtos
{
    /// <summary>
    /// DTO utilizado para receber e validar os dados de um comportamento detectado pelo sistema de visão computacional do SpaceCare.
    /// </summary>
    public class CadastrarComportamento
    {
        /// <summary>
        /// Define o identificador único do turista espacial ao qual este comportamento pertence.
        /// </summary>
        /// <value>O ID numérico do turista, sendo um campo obrigatório para vinculação do registro.</value>
        [Required(ErrorMessage = "O ID do turista é obrigatório.")]
        public int TuristaId { get; set; }

        /// <summary>
        /// Define a duração total do gesto detectado.
        /// </summary>
        /// <value>A duração medida em segundos, validada estritamente na faixa de 1 a 3600 segundos (1 hora).</value>
        [Required(ErrorMessage = "A duração do gesto é obrigatória.")]
        [Range(1, 3600, ErrorMessage = "A duração do gesto deve ser de pelo menos {1} segundo.")]
        public int DuracaoGestoSegundos { get; set; }

        /// <summary>
        /// Define o tipo de gesto detectado pelo sistema de visão computacional.
        /// </summary>
        /// <value>O valor correspondente ao enumerador <see cref="Enums.GestoDetectado"/>, validado contra os tipos aceitos pelo sistema.</value>
        [Required(ErrorMessage = "O gesto detectado é obrigatório.")]
        public GestoDetectado GestoDetectado { get; set; }
    }
}
