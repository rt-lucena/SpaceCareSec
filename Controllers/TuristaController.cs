using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceCare.Domain.Turistas.Dtos;
using SpaceCare.Services;

namespace SpaceCare.Controllers
{
    /// <summary>
    /// Controller responsável por expor os endpoints relacionados à gestão de Turistas Espaciais.
    /// </summary>
    [ApiController]
    [Route("turistas")]
    public class TuristaController : ControllerBase
    {
        private readonly TuristaService _service;

        /// <summary>
        /// Inicializa uma nova instância do <see cref="TuristaController"/>.
        /// </summary>
        /// <param name="service">O serviço de domínio dos turistas injetado.</param>
        public TuristaController(TuristaService service)
        {
            _service = service;
        }

        /// <summary>
        /// Endpoint para cadastrar um novo turista espacial no sistema.
        /// </summary>
        /// <param name="dados">Contrato com as informações obrigatórias para o registro.</param>
        /// <returns>O objeto detalhado do turista que acabou de ser persistido na base.</returns>
        /// <response code="201">Retornado quando o turista é criado com sucesso.</response>
        /// <response code="400">Retornado se as validações do modelo de dados falharem.</response>
        /// <response code="409">Retornado caso ocorra violação de unicidade de dados (Ex: passaporte duplicado).</response>
        [HttpPost]
        public async Task<IActionResult> CadastrarTurista([FromBody] CadastrarTurista dados)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var turistaCadastrado = await _service.CadastrarTurista(dados);
                return CreatedAtAction(nameof(DetalharTurista), new { id = turistaCadastrado.Id }, turistaCadastrado);
            }
            catch (DbUpdateException)
            {
                return Conflict(new { mensagem = "Já existe um turista cadastrado com este passaporte espacial." });
            }
        }

        /// <summary>
        /// Endpoint para listar todos os turistas espaciais ativos no sistema.
        /// </summary>
        /// <returns>Uma lista contendo dados resumidos de identificação dos turistas.</returns>
        /// <response code="200">Operação executada com sucesso.</response>
        [HttpGet]
        public async Task<IActionResult> ListarTuristas()
        {
            var lista = await _service.ListarTuristas();
            return Ok(lista);
        }

        /// <summary>
        /// Endpoint para detalhar todas as informações de um turista espacial específico.
        /// </summary>
        /// <param name="id">O identificador numérico exclusivo do turista.</param>
        /// <returns>As propriedades detalhadas do turista.</returns>
        /// <response code="200">Retornado quando o turista é encontrado ativo no banco.</response>
        /// <response code="404">Retornado pelo Middleware caso o turista não exista ou seja removido.</response>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> DetalharTurista(int id)
        {
            var turista = await _service.DetalharTurista(id);
            return Ok(turista);
        }

        /// <summary>
        /// Endpoint para atualizar as informações de um turista espacial já cadastrado no sistema.
        /// </summary>
        /// <param name="dados">Contrato contendo o ID alvo obrigatório e os campos de modificação.</param>
        /// <returns>O objeto detalhado do turista atualizado.</returns>
        /// <response code="200">Retornado se a alteração for realizada e salva com sucesso.</response>
        /// <response code="400">Retornado se os metadados do payload falharem na validação estrutural.</response>
        /// <response code="404">Retornado pelo Middleware caso o registro alvo não seja encontrado.</response>
        /// <response code="409">Retornado se a alteração violar e-mail ou passaporte já em uso por outro passageiro.</response>
        [HttpPut]
        public async Task<IActionResult> AtualizarTurista([FromBody] AtualizarTurista dados)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var turistaAtualizado = await _service.AtualizarTurista(dados);
                if (turistaAtualizado == null) return NotFound(new { mensagem = "Turista não encontrado." });
                return Ok(turistaAtualizado);
            }
            catch (DbUpdateException)
            {
                return Conflict(new { mensagem = "O e-mail ou passaporte informado já está em uso por outro passageiro." });
            }
        }

        /// <summary>
        /// Endpoint para excluir logicamente um turista espacial do sistema, alterando seu estado.
        /// </summary>
        /// <param name="id">O identificador numérico único do turista.</param>
        /// <returns>Uma resposta vazia com status de sucesso HTTP 204.</returns>
        /// <response code="204">Exclusão processada com sucesso no banco de dados.</response>
        /// <response code="404">Retornado pelo Middleware caso o ID do turista não seja localizado ativo.</response>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> ExcluirTurista(int id)
        {
            await _service.ExcluirTurista(id);
            return NoContent();
        }
    }
}
