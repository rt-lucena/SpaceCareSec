using SpaceCare.Domain.Turistas;
using SpaceCare.Domain.Turistas.Dtos;
using SpaceCare.Infra.Data;
using Microsoft.EntityFrameworkCore;
using SpaceCare.Domain.Exceptions;
using SpaceCare.Domain.Turistas.Interfaces;

namespace SpaceCare.Services
{
    /// <summary>
    /// Service responsável por gerenciar as operações relacionadas aos Turistas Espaciais,
    /// incluindo cadastro, listagem, detalhamento, atualização e exclusão lógica.
    /// </summary>
    public class TuristaService : ITuristaService
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Inicializa uma nova instância do <see cref="TuristaService"/> com o contexto de banco de dados injetado.
        /// </summary>
        /// <param name="context">O contexto de banco de dados.</param>
        public TuristaService(AppDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<DetalharTurista> CadastrarTurista(CadastrarTurista dados)
        {
            var turista = new Turista
            {
                Nome = dados.Nome,
                PassaporteEspacial = dados.PassaporteEspacial,
                DataNascimento = dados.DataNascimento,
                Email = dados.Email,
                HistoricoMedico = dados.HistoricoMedico,
                DataCadastro = DateTime.UtcNow,
                Ativo = "1"
            };

            _context.Turistas.Add(turista);
            await _context.SaveChangesAsync();

            return new DetalharTurista(
                turista.Id,
                turista.Nome,
                turista.PassaporteEspacial,
                turista.DataNascimento,
                turista.Email,
                turista.HistoricoMedico,
                turista.DataCadastro
            );
        }

        /// <inheritdoc/>
        public async Task<List<ListagemTurista>> ListarTuristas()
        {
            return await _context.Turistas
                .Where(t => t.Ativo == "1")
                .Select(t => new ListagemTurista(t.Id, t.Nome, t.Email))
                .ToListAsync();
        }

        /// <inheritdoc/>
        /// <exception cref="TuristaNotFoundException">Disparada caso o ID informado não exista.</exception>
        public async Task<DetalharTurista> DetalharTurista(int id)
        {
            var turista = await _context.Turistas
                .FirstOrDefaultAsync(t => t.Id == id && t.Ativo == "1")
                ?? throw new TuristaNotFoundException("ID do turista informado não existe!");

            return new DetalharTurista(
                turista.Id,
                turista.Nome,
                turista.PassaporteEspacial,
                turista.DataNascimento,
                turista.Email,
                turista.HistoricoMedico,
                turista.DataCadastro
            );
        }

        /// <inheritdoc/>
        /// <exception cref="TuristaNotFoundException">Disparada caso o ID informado não exista.</exception>
        public async Task<DetalharTurista> AtualizarTurista(AtualizarTurista dados)
        {
            var turista = await _context.Turistas
                .FirstOrDefaultAsync(t => t.Id == dados.Id && t.Ativo == "1")
                ?? throw new TuristaNotFoundException("ID do turista informado não existe!");

            if (!string.IsNullOrWhiteSpace(dados.Nome))
                turista.Nome = dados.Nome;

            if (!string.IsNullOrWhiteSpace(dados.PassaporteEspacial))
                turista.PassaporteEspacial = dados.PassaporteEspacial;

            if (!string.IsNullOrWhiteSpace(dados.Email))
                turista.Email = dados.Email;

            if (!string.IsNullOrWhiteSpace(dados.HistoricoMedico))
                turista.HistoricoMedico = dados.HistoricoMedico;

            await _context.SaveChangesAsync();

            return new DetalharTurista(
                turista.Id,
                turista.Nome,
                turista.PassaporteEspacial,
                turista.DataNascimento,
                turista.Email,
                turista.HistoricoMedico,
                turista.DataCadastro
            );
        }

        /// <inheritdoc/>
        /// <exception cref="TuristaNotFoundException">Disparada caso o ID informado não exista ou já tenha sido excluído.</exception>
        public async Task ExcluirTurista(int id)
        {
            var turista = await _context.Turistas
                .FirstOrDefaultAsync(t => t.Id == id && t.Ativo == "1")
                ?? throw new TuristaNotFoundException("ID do turista informado não existe!"); ;

            turista.Ativo = "0";
            await _context.SaveChangesAsync();
        }
    }
}
