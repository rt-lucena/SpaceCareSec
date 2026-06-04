using Microsoft.AspNetCore.Mvc;
using SpaceCare.Domain.Comportamentos.Dtos;
using SpaceCare.Services;

namespace SpaceCare.Controllers
{
    [ApiController]
    [Route("comportamentos")]
    public class ComportamentoController : ControllerBase
    {
        private readonly ComportamentoService _service;

        public ComportamentoController(ComportamentoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarComportamento([FromBody] CadastrarComportamento dados)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var resultado = await _service.CadastrarComportamentoAsync(dados);
                return CreatedAtAction(nameof(ListarPorTurista), new { turistaId = resultado.TuristaId }, resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet("turista/{turistaId:int}")]
        public async Task<IActionResult> ListarPorTurista(int turistaId)
        {
            var historico = await _service.ListarComportamentosPorTurista(turistaId);
            if (historico == null)
                return NotFound(new { mensagem = "Histórico indisponível, o turista informado não existe." });

            return Ok(historico);
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var comportamentos = await _service.ListarTodosAsync();
            return Ok(comportamentos);
        }
    }
}
