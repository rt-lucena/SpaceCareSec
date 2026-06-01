namespace SpaceCare.Domain.Turistas.Dtos
{
    public record DetalharTurista(
        int Id,
        string Nome,
        string PassaporteEspacial,
        DateTime DataNascimento,
        string Email,
        string? HistoricoMedico,
        DateTime DataCadastro
    );
}
