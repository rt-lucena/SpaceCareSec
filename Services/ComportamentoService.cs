using Microsoft.EntityFrameworkCore;
using SpaceCare.Domain.Comportamentos;
using SpaceCare.Domain.Comportamentos.Dtos;
using SpaceCare.Domain.Comportamentos.Enums;
using SpaceCare.Infra.Data;

namespace SpaceCare.Services
{
    public class ComportamentoService
    {
        private readonly AppDbContext _context;

        public ComportamentoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DetalharComportamento?> CadastrarComportamentoAsync(CadastrarComportamento dados)
        {
            var turista = await _context.Turistas.FirstOrDefaultAsync(t => t.Id == dados.TuristaId && t.Ativo == "1");
            if (turista == null)
                throw new Exception("Turista não encontrado no sistema.");
            if (turista.Ativo != "1")
                throw new Exception("Não é possível registrar comportamentos para um turista inativo.");

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

        public async Task<List<DetalharComportamento>?> ListarComportamentosPorTurista(int turistaId)
        {
            var turistaAtivo = await _context.Turistas.FirstOrDefaultAsync(t => t.Id == turistaId && t.Ativo == "1");
            if (turistaAtivo == null) return null;

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

        public async Task<List<DetalharComportamento>?> ListarTodosAsync()
        {
            return await _context.Comportamentos
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
