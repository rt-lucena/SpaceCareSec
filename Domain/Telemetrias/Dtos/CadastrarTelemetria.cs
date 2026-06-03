
using System.ComponentModel.DataAnnotations;

namespace SpaceCare.Domain.Telemetrias.Dtos
{
    public class CadastrarTelemetria
    {
        [Required(ErrorMessage = "O ID do turista é obrigatório para vincular a telemetria.")]
        public int TuristaId { get; set; }

        [Required(ErrorMessage = "A quantidade de batimentos cardíacos é obrigatória.")]
        [Range(30, 250, ErrorMessage = "Os batimentos cardíacos devem estar entre {1} e {2} BPM.")]
        public int BatimentosCardiacos { get; set; }

        [Required(ErrorMessage = "A temperatura corporal é obrigatória.")]
        [Range(30.0, 45.0, ErrorMessage = "A temperatura corporal deve estar entre {1}°C e {2}°C.")]
        public decimal TemperaturaCorporal { get; set; }

        [Required(ErrorMessage = "A aferição da pressão arterial é obrigatória.")]
        [StringLength(10, ErrorMessage = "A descrição da pressão arterial não pode passar de {1} caracteres.")]
        public string PressaoArterial { get; set; } = string.Empty;
    }
}
