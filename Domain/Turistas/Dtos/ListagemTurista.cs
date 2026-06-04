namespace SpaceCare.Domain.Turistas.Dtos
{
    /// <summary>
    /// DTO estruturado como imutável (<see langword="record"/>),
    /// utilizado para expor as informações básicas de um Turista Espacial nas listagens e respostas resumidas da API.
    /// </summary>
    /// <param name="Id">O identificador único do turista.</param>
    /// <param name="Nome">O nome completo do turista.</param>
    /// <param name="Email">O endereço de e-mail do turista.</param>
    public record ListagemTurista(
        int Id,
        string Nome,
        string Email
    );
}
