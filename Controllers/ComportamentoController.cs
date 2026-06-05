using Microsoft.AspNetCore.Mvc;
using SpaceCare.Domain.Comportamentos.Dtos;
using SpaceCare.Domain.Comportamentos.Interfaces;

namespace SpaceCare.Controllers
{
    /// <summary>
    /// Controller responsável por expor os endpoints relacionados à gestão dos comportamentos detectados dos Turistas Espaciais,
    /// </summary>
    [ApiController]
    [Route("comportamentos")]
    public class ComportamentoController : ControllerBase
    {
        private readonly IComportamentoService _service;

        /// <summary>
        /// Inicializa uma nova instância do <see cref="ComportamentoController"/>.
        /// </summary>
        /// <param name="service">O serviço de domínio de comportamentos injetado.</param>
        public ComportamentoController(IComportamentoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Endpoint para cadastrar um novo comportamento detectado de um turista espacial. 
        /// </summary>
        /// <param name="dados">Contrato contendo as propriedades do gesto detectado e sua respectiva duração.</param>
        /// <returns>O objeto detalhado do comportamento processado e persistido no banco</returns>
        /// <response code="201">Retornado quando o comportamento é computado e salvo com sucesso.</response>
        /// <response code="400">Retornado se houver falhas de validação estrutural nos metadados enviados.</response>
        /// <response code="404">Retornado pelo Middleware caso o turista espacial vinculado não exista.</response>
        [HttpPost]
        public async Task<IActionResult> CadastrarComportamento([FromBody] CadastrarComportamento dados)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

             var resultado = await _service.CadastrarComportamentoAsync(dados);
             return CreatedAtAction(nameof(ListarPorTurista), new { turistaId = resultado.TuristaId }, resultado);
        }

        /// <summary>
        /// Endpoint para listar o histórico completo de comportamentos e alertas de um turista espacial específico
        /// </summary>
        /// <param name="turistaId">O identificador numérico único do turista.</param>
        /// <returns>Uma lista contendo todos comportamentos do usuário ordenados pela data do registro mais recente.</returns>
        /// <response code="200">Operação executada com sucesso.</response>
        /// <response code="404">Retornado pelo Middleware caso o turista não exista ou seja removido.</response>
        [HttpGet("turista/{turistaId:int}")]
        public async Task<IActionResult> ListarPorTurista(int turistaId)
        {
            var historico = await _service.ListarComportamentosPorTurista(turistaId);
            return Ok(historico);
        }

        /// <summary>
        /// Endpoint para listar todos os comportamentos e alertas registrados no sistema.
        /// </summary>
        /// <returns>Uma lista contendo todos os comportamentos ordenados pela data do registro mais recente.</returns>
        /// <response code="200">Operação executada com sucesso.</response>
        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var comportamentos = await _service.ListarTodosAsync();
            return Ok(comportamentos);
        }
    }
}
