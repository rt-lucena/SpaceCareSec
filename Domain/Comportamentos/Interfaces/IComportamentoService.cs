using SpaceCare.Domain.Comportamentos.Dtos;

namespace SpaceCare.Domain.Comportamentos.Interfaces
{
    /// <summary>
    /// Interface que define os contratos para o serviço de gerenciamento dos comportamentos dos turistas espaciais.
    /// </summary>
    public interface IComportamentoService
    {
        /// <summary>
        /// Registra um novo comportamento detectado para um turista específico, 
        /// avaliando o nível de alerta com base no gesto detectado e nas últimas telemetrias disponíveis.
        /// </summary>
        /// <param name="dados">Contrato com as informações obrigatórias para registro do comportamento.</param>
        /// <returns>Os dados consolidados do comportamento registrado e seu respectivo nível de triagem.</returns>
        Task<DetalharComportamento> CadastrarComportamentoAsync(CadastrarComportamento dados);

        /// <summary>
        /// Lista os comportamentos detectados para um turista específico, ordenados da leitura mais recente para a mais antiga.
        /// </summary>
        /// <param name="turistaId">O identificador único do turista alvo da pesquisa.</param>
        /// <returns>Uma lista ordenada de forma decrescente por data contendo os registros <see cref="DetalharComportamento"/>.</returns>
        Task<List<DetalharComportamento>> ListarComportamentosPorTurista(int turistaId);

        /// <summary>
        /// Lista o histórico de comportamentos de todos os turistas espaciais ativos.
        /// </summary>
        /// <returns>Uma lista ordenada de forma decrescente por data contendo os registros <see cref="DetalharComportamento"/>.</returns>
        Task<List<DetalharComportamento>> ListarTodosAsync();

    }
}
