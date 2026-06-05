using SpaceCare.Domain.Turistas.Dtos;

namespace SpaceCare.Domain.Turistas.Interfaces
{
    /// <summary>
    /// Interface que define os contratos para o serviço de gerenciamento de turistas espaciais.
    /// </summary>
    public interface ITuristaService
    {
        /// <summary>
        /// Cadastra um novo turista espacial no sistema, salvando suas informações no banco de dados.
        /// </summary>
        /// <param name="dados">Objeto contendo os dados necessários para o cadastro.</param>
        /// <returns>Um <see cref="DetalharTurista"/> contendo os dados do registro criado e o ID persistido.</returns>
        Task<DetalharTurista> CadastrarTurista(CadastrarTurista dados);

        /// <summary>
        /// Lista resumidamente informações de todos os turistas espaciais ativos no sistema.
        /// </summary>
        /// <returns>Uma lista contendo registros do tipo <see cref="ListagemTurista"/>.</returns>
        Task<List<ListagemTurista>> ListarTuristas();

        /// <summary>
        /// Obtém as informações completas de um turista espacial específico, caso esteja ativo.
        /// </summary>
        /// <param name="id">O identificador único do turista</param>
        /// <returns>O registro <see cref="DetalharTurista"/> preenchido com os dados do banco</returns>
        Task<DetalharTurista> DetalharTurista(int id);

        /// <summary>
        /// Atualiza as informações de um turista espacial existente, permitindo a modificação de campos específicos.
        /// </summary>
        /// <param name="dados">Contrato contendo o ID do registro alvo e os campos modificados.</param>
        /// <returns>O registro <see cref="DetalharTurista"/> atualizado refletindo o novo estado no banco</returns>
        Task<DetalharTurista> AtualizarTurista(AtualizarTurista dados);

        /// <summary>
        /// Realiza a exclusão lógica de um turista espacial, marcando seu registro como inativo no banco de dados.
        /// </summary>
        /// <param name="id">O identificador único do turista</param>
        /// <returns>Uma <see cref="Task"/> representando a conclusão assíncrona da operação.</returns>
        Task ExcluirTurista(int id);
    }
}
