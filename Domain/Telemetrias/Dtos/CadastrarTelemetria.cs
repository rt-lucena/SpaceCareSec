using System.ComponentModel.DataAnnotations;

namespace SpaceCare.Domain.Telemetrias.Dtos
{
    /// <summary>
    /// DTO utilizado para receber e validar as leituras enviada pelos sensores.
    /// </summary>
    public class CadastrarTelemetria
    {
        /// <summary>
        /// Define o identificador único do turista espacial ao qual esta telemetria pertence.
        /// </summary>
        /// <value>O ID numérico do turista, sendo um campo obrigatório para vinculação do registro.</value>
        [Required(ErrorMessage = "O ID do turista é obrigatório para vincular a telemetria.")]
        public int TuristaId { get; set; }

        /// <summary>
        /// Define a frequência cardíaca registrada do turista.
        /// </summary>
        /// <value>Os batimentos em BPM, obrigatórios e validados para a faixa fisiológica aceitável de 30 a 250 BPM.</value>
        [Required(ErrorMessage = "A quantidade de batimentos cardíacos é obrigatória.")]
        [Range(30, 250, ErrorMessage = "Os batimentos cardíacos devem estar entre {1} e {2} BPM.")]
        public int BatimentosCardiacos { get; set; }

        /// <summary>
        /// Define a temperatura corporal do turista.
        /// </summary>
        /// <value>A temperatura em graus Celsius, obrigatória e validada para a faixa aceitável de 30.0°C a 45.0°C.</value>
        [Required(ErrorMessage = "A temperatura corporal é obrigatória.")]
        [Range(30.0, 45.0, ErrorMessage = "A temperatura corporal deve estar entre {1}°C e {2}°C.")]
        public decimal TemperaturaCorporal { get; set; }

        /// <summary>
        /// Define os dados textuais da aferição de pressão arterial (ex: "12/8").
        /// </summary>
        /// <value>A string descritiva da pressão, obrigatória e limitada ao comprimento máximo de 10 caracteres.</value>
        [Required(ErrorMessage = "A aferição da pressão arterial é obrigatória.")]
        [StringLength(10, ErrorMessage = "A descrição da pressão arterial não pode passar de {1} caracteres.")]
        public string PressaoArterial { get; set; } = string.Empty;
    }
}
