using SpaceCare.Domain.Comportamentos.Enums;
using System.ComponentModel.DataAnnotations;

namespace SpaceCare.Domain.Comportamentos.Dtos
{
    public class CadastrarComportamento
    {
        [Required(ErrorMessage = "O ID do turista é obrigatório.")]
        public int TuristaId { get; set; }

        [Required(ErrorMessage = "A duração do gesto é obrigatória.")]
        [Range(1, 3600, ErrorMessage = "A duração do gesto deve ser de pelo menos {1} segundo.")]
        public int DuracaoGestoSegundos { get; set; }

        [Required(ErrorMessage = "O gesto detectado é obrigatório.")]
        [EnumDataType(typeof(GestoDetectado), ErrorMessage = "Gesto inválido para o sistema SpaceCare.")]
        public GestoDetectado GestoDetectado { get; set; }
    }
}
