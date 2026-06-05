using Microsoft.EntityFrameworkCore;
using SpaceCare.Domain.Comportamentos;
using SpaceCare.Domain.Comportamentos.Dtos;
using SpaceCare.Domain.Comportamentos.Enums;
using SpaceCare.Domain.Exceptions;
using SpaceCare.Infra.Data;

namespace SpaceCare.Services
{
    /// <summary>
    /// Service responsável por gerenciar as operações relacionadas aos comportamentos detectados dos Turistas Espaciais.
    /// </summary>
    public class ComportamentoService
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Inicializa uma nova instância do <see cref="ComportamentoService"/>.
        /// </summary>
        /// <param name="context">O contexto de banco de dados</param>
        public ComportamentoService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Registra um novo comportamento detectado para um turista específico, 
        /// avaliando o nível de alerta com base no gesto detectado e nas últimas telemetrias disponíveis.
        /// </summary>
        /// <param name="dados">Contrato com as informações obrigatórias para registro do comportamento.</param>
        /// <returns>Os dados consolidados do comportamento registrado e seu respectivo nível de triagem.</returns>
        /// <exception cref="TuristaNotFoundException">Disparada caso o ID do turista informado não exista.</exception>
        public async Task<DetalharComportamento> CadastrarComportamentoAsync(CadastrarComportamento dados)
        {
            _ = await _context.Turistas
                .FirstOrDefaultAsync(t => t.Id == dados.TuristaId && t.Ativo == "1")
                ?? throw new TuristaNotFoundException("ID do turista informado não existe!");

            if (!Enum.IsDefined(typeof(GestoDetectado), dados.GestoDetectado))
                throw new ArgumentException("Gesto inválido para o sistema SpaceCare.");

            var ultimaTelemetria = await _context.Telemetrias
                .Where(t => t.TuristaId == dados.TuristaId)
                .OrderByDescending(t => t.DataLeitura)
                .FirstOrDefaultAsync();

            NivelAlerta alerta = NivelAlerta.Verde;

            if (dados.GestoDetectado == GestoDetectado.PolegarParaCima)
                alerta = NivelAlerta.Verde;
            else if (dados.GestoDetectado == GestoDetectado.PolegarParaBaixo)
                alerta = NivelAlerta.Amarelo;
            else if (dados.GestoDetectado == GestoDetectado.MaoNaCabeca || dados.GestoDetectado == GestoDetectado.MaoNoPeito)
            {
                if (ultimaTelemetria != null)
                {
                    int batimentos = ultimaTelemetria.BatimentosCardiacos;
                    decimal temperatura = ultimaTelemetria.TemperaturaCorporal;

                    int pressaoSistolica = 0;
                    if (!string.IsNullOrEmpty(ultimaTelemetria.PressaoArterial))
                    {
                        var partesPressao = ultimaTelemetria.PressaoArterial.Split('/');
                        if (partesPressao.Length == 2)
                            int.TryParse(partesPressao[0], out pressaoSistolica);
                    }

                    if (dados.GestoDetectado == GestoDetectado.MaoNoPeito)
                    {
                        bool pressaoAlta = pressaoSistolica >= 140 || (pressaoSistolica >= 14 && pressaoSistolica < 20);

                        if (pressaoAlta || batimentos >= 140)
                            alerta = NivelAlerta.Vermelho;
                        else
                            alerta = NivelAlerta.Amarelo;
                    }
                    else if (dados.GestoDetectado == GestoDetectado.MaoNaCabeca)
                    {
                        bool pressaoBaixa = (pressaoSistolica > 20 && pressaoSistolica <= 90) || (pressaoSistolica > 0 && pressaoSistolica <= 9);
                        bool pressaoAlta = pressaoSistolica >= 160 || (pressaoSistolica >= 16 && pressaoSistolica < 20);

                        if (pressaoBaixa || pressaoAlta || temperatura >= 38)
                            alerta = NivelAlerta.Vermelho;
                        else
                            alerta = NivelAlerta.Amarelo;
                    }
                }
                else
                    alerta = NivelAlerta.Amarelo;
            }

            var comportamento = new Comportamento
            {
                TuristaId = dados.TuristaId,
                DuracaoGestoSegundos = dados.DuracaoGestoSegundos,
                GestoDetectado = dados.GestoDetectado,
                NivelAlerta = alerta,
                DataLeitura = DateTime.UtcNow
            };

            _context.Comportamentos.Add(comportamento);
            await _context.SaveChangesAsync();

            return new DetalharComportamento(
                comportamento.Id,
                comportamento.TuristaId,
                comportamento.DuracaoGestoSegundos,
                comportamento.GestoDetectado,
                comportamento.NivelAlerta,
                comportamento.DataLeitura
            );
        }

        /// <summary>
        /// Lista os comportamentos detectados para um turista específico, ordenados da leitura mais recente para a mais antiga.
        /// </summary>
        /// <param name="turistaId">O identificador único do turista alvo da pesquisa.</param>
        /// <returns>Uma lista ordenada de forma decrescente por data contendo os registros <see cref="DetalharComportamento"/>.</returns>
        /// <exception cref="TuristaNotFoundException">Disparada caso o ID do turista informado não exista.</exception>
        public async Task<List<DetalharComportamento>> ListarComportamentosPorTurista(int turistaId)
        {
            _ = await _context.Turistas
                .FirstOrDefaultAsync(t => t.Id == turistaId && t.Ativo == "1")
                ?? throw new TuristaNotFoundException("ID do turista informado não existe!");

            return await _context.Comportamentos
                .Where(c => c.TuristaId == turistaId)
                .OrderByDescending(c => c.DataLeitura)
                .Select(c => new DetalharComportamento(
                    c.Id,
                    c.TuristaId,
                    c.DuracaoGestoSegundos,
                    c.GestoDetectado,
                    c.NivelAlerta,
                    c.DataLeitura
                ))
                .ToListAsync();
        }

        /// <summary>
        /// Lista o histórico de comportamentos de todos os turistas espaciais ativos.
        /// </summary>
        /// <returns>Uma lista ordenada de forma decrescente por data contendo os registros <see cref="DetalharComportamento"/>.</returns>
        public async Task<List<DetalharComportamento>> ListarTodosAsync()
        {
            return await _context.Comportamentos
                .Include(c => c.Turista)
                .Where(c => c.Turista!.Ativo == "1")
                .OrderByDescending(c => c.DataLeitura)
                .Select(c => new DetalharComportamento(
                    c.Id,
                    c.TuristaId,
                    c.DuracaoGestoSegundos,
                    c.GestoDetectado,
                    c.NivelAlerta,
                    c.DataLeitura
                ))
                .ToListAsync();
        }
    }
}
