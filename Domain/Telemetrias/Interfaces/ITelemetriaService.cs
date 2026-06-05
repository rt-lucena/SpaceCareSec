using SpaceCare.Domain.Telemetrias.Dtos;

namespace SpaceCare.Domain.Telemetrias.Interfaces
{
    /// <summary>
    /// Interface que define os contratos para o serviço de gerenciamento das telemetrias dos turistas espaciais.
    /// </summary>
    public interface ITelemetriaService
    {
        /// <summary>
        /// Cadastra uma nova leitura de telemetria a um turista espacial ativo.
        /// </summary>
        /// <param name="dados">Contrato contendo as aferições dos sinais vitais capturados pelos sensores.</param>
        /// <returns>Um <see cref="DetalharTelemetria"/> preenchido com os dados e o ID persistido.</returns>
        Task<DetalharTelemetria> CadastrarTelemetria(CadastrarTelemetria dados);

        /// <summary>
        /// Lista o histórico de telemetrias de um turista espacial específico.
        /// </summary>
        /// <param name="turistaId">O identificador único do turista alvo da pesquisa.</param>
        /// <returns>Uma lista ordenada de forma decrescente por data contendo os registros <see cref="DetalharTelemetria"/>.</returns>
        Task<List<DetalharTelemetria>> ListarHistoricoPorTurista(int turistaId);

        /// <summary>
        /// Lista o histórico de telemetrias de todos os turistas espaciais ativos.
        /// </summary>
        /// <returns>Uma lista ordenada de forma decrescente por data contendo os registros <see cref="DetalharTelemetria"/>.</returns>
        Task<List<DetalharTelemetria>> ListarTodasTelemetrias();
    }
}
