using SpaceCare.Domain.Telemetrias;
using SpaceCare.Domain.Telemetrias.Dtos;
using SpaceCare.Infra.Data;
using Microsoft.EntityFrameworkCore;
using SpaceCare.Domain.Exceptions;

namespace SpaceCare.Services
{
    /// <summary>
    /// Service responsável por gerenciar as operações relacionadas às telemetrias dos Turistas Espaciais.
    /// </summary>
    public class TelemetriaService
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Inicializa uma nova instância do <see cref="TelemetriaService"/>.
        /// </summary>
        /// <param name="context">O contexto de banco de dados</param>
        public TelemetriaService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Cadastra uma nova leitura de telemetria a um turista espacial ativo.
        /// </summary>
        /// <param name="dados">Contrato contendo as aferições dos sinais vitais capturados pelos sensores.</param>
        /// <returns>Um <see cref="DetalharTelemetria"/> preenchido com os dados e o ID persistido.</returns>
        /// <exception cref="TuristaNotFoundException">Disparada caso o ID do turista informado não exista.</exception>
        /// <exception cref="ArgumentException">Disparada caso a descrição da pressão arterial não contenha o caractere delimitador '/'.</exception>
        public async Task<DetalharTelemetria> CadastrarTelemetria(CadastrarTelemetria dados)
        {
            _ = await _context.Turistas
                .FirstOrDefaultAsync(t => t.Id == dados.TuristaId && t.Ativo == "1")
                ?? throw new TuristaNotFoundException("ID do turista informado não existe!");

            if (!dados.PressaoArterial.Contains("/"))
                throw new ArgumentException("A pressão arterial precisa conter o caractere '/' fazendo separação.");

            var telemetria = new Telemetria
            {
                TuristaId = dados.TuristaId,
                BatimentosCardiacos = dados.BatimentosCardiacos,
                TemperaturaCorporal = dados.TemperaturaCorporal,
                PressaoArterial = dados.PressaoArterial,
                DataLeitura = DateTime.UtcNow 
            };

            _context.Telemetrias.Add(telemetria);
            await _context.SaveChangesAsync();

            return new DetalharTelemetria(
                telemetria.Id,
                telemetria.TuristaId,
                telemetria.BatimentosCardiacos,
                telemetria.TemperaturaCorporal,
                telemetria.PressaoArterial,
                telemetria.DataLeitura
            );
        }

        /// <summary>
        /// Lista o histórico de telemetrias de um turista espacial específico.
        /// </summary>
        /// <param name="turistaId">O identificador único do turista alvo da pesquisa.</param>
        /// <returns>Uma lista ordenada de forma decrescente por data contendo os registros <see cref="DetalharTelemetria"/>.</returns>
        /// <exception cref="TuristaNotFoundException">Disparada caso o ID do turista informado não exista.</exception>
        public async Task<List<DetalharTelemetria>> ListarHistoricoPorTurista(int turistaId)
        {
            var turistaAtivo = await _context.Turistas
                .FirstOrDefaultAsync(t => t.Id == turistaId && t.Ativo == "1")
                ?? throw new TuristaNotFoundException("ID do turista informado não existe!");

            return await _context.Telemetrias
                .Where(t => t.TuristaId == turistaId)
                .OrderByDescending(t => t.DataLeitura)
                .Select(t => new DetalharTelemetria(
                    t.Id,
                    t.TuristaId,
                    t.BatimentosCardiacos,
                    t.TemperaturaCorporal,
                    t.PressaoArterial,
                    t.DataLeitura
                ))
                .ToListAsync();
        }

        /// <summary>
        /// Lista o histórico de telemetrias de todos os turistas espaciais ativos.
        /// </summary>
        /// <returns>Uma lista ordenada de forma decrescente por data contendo os registros <see cref="DetalharTelemetria"/>.</returns>
        public async Task<List<DetalharTelemetria>> ListarTodasTelemetrias()
        {
            return await _context.Telemetrias
                .Include(t => t.Turista)
                .Where(t => t.Turista!.Ativo == "1") 
                .OrderByDescending(t => t.DataLeitura)
                .Select(t => new DetalharTelemetria(
                    t.Id,
                    t.TuristaId,
                    t.BatimentosCardiacos,
                    t.TemperaturaCorporal,
                    t.PressaoArterial,
                    t.DataLeitura
                ))
                .ToListAsync();
        }
    }
}
