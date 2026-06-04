using SpaceCare.Domain.Telemetrias;
using SpaceCare.Domain.Telemetrias.Dtos;
using SpaceCare.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace SpaceCare.Services
{
    public class TelemetriaService
    {
        private readonly AppDbContext _context;

        public TelemetriaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DetalharTelemetria?> CadastrarTelemetria(CadastrarTelemetria dados)
        {
            var turistaExiste = await _context.Turistas.FirstOrDefaultAsync(t => t.Id == dados.TuristaId && t.Ativo == "1");
            if (turistaExiste == null) return null;

            if (!dados.PressaoArterial.Contains("/"))
                throw new BadHttpRequestException("A pressão arterial precisa conter o caractere '/' fazendo separação.");

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

        public async Task<List<DetalharTelemetria>?> ListarHistoricoPorTurista(int turistaId)
        {
            var turistaAtivo = await _context.Turistas.FirstOrDefaultAsync(t => t.Id == turistaId && t.Ativo == "1");
            if (turistaAtivo == null) return null;

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
