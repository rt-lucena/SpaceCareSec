using SpaceCare.Domain.Turistas;
using SpaceCare.Domain.Turistas.Dtos;
using SpaceCare.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace SpaceCare.Services
{
    public class TuristaService
    {
        private readonly AppDbContext _context;

        public TuristaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Turista> CadastrarTurista(CadastroTuristaDto dados)
        {
            var turista = new Turista
            {
                Nome = dados.Nome,
                PassaporteEspacial = dados.PassaporteEspacial,
                DataNascimento = dados.DataNascimento,
                Email = dados.Email,
                HistoricoMedico = dados.HistoricoMedico,
                DataCadastro = DateTime.UtcNow
            };

            _context.Turistas.Add(turista);
            await _context.SaveChangesAsync();

            return turista;
        }

        public async Task<List<ListagemTurista>> ListarTuristas()
        {
            return await _context.Turistas
                .Select(t => new ListagemTurista(t.Id, t.Nome, t.Email))
                .ToListAsync();
        }

        public async Task<DetalharTurista?> DetalharTurista(int id)
        {
            var turista = await _context.Turistas.FindAsync(id);
            if (turista == null) return null;

            return new DetalharTurista(
                Id: turista.Id,
                Nome: turista.Nome,
                PassaporteEspacial: turista.PassaporteEspacial,
                DataNascimento: turista.DataNascimento,
                Email: turista.Email,
                HistoricoMedico: turista.HistoricoMedico,
                DataCadastro: turista.DataCadastro
            );
        }
    }
}
