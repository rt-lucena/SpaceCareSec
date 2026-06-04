using SpaceCare.Domain.Turistas;
using SpaceCare.Domain.Turistas.Dtos;
using SpaceCare.Infra.Data;
using Microsoft.EntityFrameworkCore;
using SpaceCare.Domain.Exceptions;

namespace SpaceCare.Services
{
    /// <summary>
    /// Service responsável por gerenciar as operações relacionadas aos Turistas Espaciais,
    /// incluindo cadastro, listagem, detalhamento, atualização e exclusão lógica.
    /// </summary>
    public class TuristaService
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

        /// <summary>
        /// Cadastra um novo turista espacial no sistema, salvando suas informações no banco de dados.
        /// </summary>
        /// <param name="dados">Objeto contendo os dados necessários para o cadastro.</param>
        /// <returns>Um <see cref="DetalharTurista"/> contendo os dados do registro criado e o ID persistido.</returns>
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

        /// <summary>
        /// Lista resumidamente informações de todos os turistas espaciais ativos no sistema.
        /// </summary>
        /// <returns>Uma lista contendo registros do tipo <see cref="ListagemTurista"/>.</returns>
        public async Task<List<ListagemTurista>> ListarTuristas()
        {
            return await _context.Turistas
                .Where(t => t.Ativo == "1")
                .Select(t => new ListagemTurista(t.Id, t.Nome, t.Email))
                .ToListAsync();
        }

        /// <summary>
        /// Obtém as informações completas de um turista espacial específico, caso esteja ativo.
        /// </summary>
        /// <param name="id">O identificador único do turista</param>
        /// <returns>O registro <see cref="DetalharTurista"/> preenchido com os dados do banco</returns>
        /// <exception cref="TuristaNotFoundException">Disparada caso o ID informado não exista.</exception>
        public async Task<DetalharTurista?> DetalharTurista(int id)
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

        /// <summary>
        /// Atualiza as informações de um turista espacial existente, permitindo a modificação de campos específicos.
        /// </summary>
        /// <param name="dados">Contrato contendo o ID do registro alvo e os campos modificados.</param>
        /// <returns>O registro <see cref="DetalharTurista"/> atualizado refletindo o novo estado no banco</returns>
        /// <exception cref="TuristaNotFoundException">Disparada caso o ID informado não exista.</exception>
        public async Task<DetalharTurista?> AtualizarTurista(AtualizarTurista dados)
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

        /// <summary>
        /// Realiza a exclusão lógica de um turista espacial, marcando seu registro como inativo no banco de dados.
        /// </summary>
        /// <param name="id">O identificador único do turista</param>
        /// <returns>Uma <see cref="Task"/> representando a conclusão assíncrona da operação.</returns>
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
