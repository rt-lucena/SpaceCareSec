using System.ComponentModel.DataAnnotations;

namespace SpaceCare.Domain.Turistas.Dtos
{
    public class CriarTuristaDto
    {
        [Required(ErrorMessage = "O nome do turista é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode passar de {1} caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O número do passaporte espacial é obrigatório.")]
        [StringLength(20, ErrorMessage = "O passaporte espacial não pode passar de {1} caracteres.")]
        public string PassaporteEspacial { get; set; } = string.Empty;

        [Required(ErrorMessage = "A idade é obrigatória.")]
        public int Idade { get; set; }

        [StringLength(250, ErrorMessage = "O histórico médico pode ter no máximo {1} caracteres.")]
        public string? HistoricoMedico { get; set; }
    }
}
