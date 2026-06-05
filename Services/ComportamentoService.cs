using Microsoft.EntityFrameworkCore;
using SpaceCare.Domain.Comportamentos;
using SpaceCare.Domain.Comportamentos.Dtos;
using SpaceCare.Domain.Comportamentos.Enums;
using SpaceCare.Domain.Comportamentos.Interfaces;
using SpaceCare.Domain.Exceptions;
using SpaceCare.Infra.Data;

namespace SpaceCare.Services
{
    /// <summary>
    /// Service responsável por gerenciar as operações relacionadas aos comportamentos detectados dos Turistas Espaciais.
    /// </summary>
    public class ComportamentoService : IComportamentoService
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
