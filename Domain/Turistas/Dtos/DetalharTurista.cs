namespace SpaceCare.Domain.Turistas.Dtos
{
    /// <summary>
    /// DTO estruturado como imutável (<see langword="record"/>),
    /// utilizado para expor os detalhes consolidados de um Turista Espacial nas respostas da API.
    /// </summary>
    /// <param name="Id">O identificador único do turista.</param>
    /// <param name="Nome">O nome completo do turista.</param>
    /// <param name="PassaporteEspacial">O número do passaporte espacial do turista.</param>
    /// <param name="DataNascimento">A data de nascimento do turista.</param>
    /// <param name="Email">O endereço de e-mail do turista.</param>
    /// <param name="HistoricoMedico">As observações clínicas, alergias ou condições crônicas do turista.</param>
    /// <param name="DataCadastro">A data e hora em que o turista foi cadastrado.</param>
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
