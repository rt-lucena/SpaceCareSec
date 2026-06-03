using Microsoft.AspNetCore.Mvc;
using SpaceCare.Domain.Telemetrias.Dtos;
using SpaceCare.Services;

namespace SpaceCare.Controllers
{
    [ApiController]
    [Route("telemetrias")]
    public class TelemetriaController : ControllerBase
    {
        private readonly TelemetriaService _service;

        public TelemetriaController(TelemetriaService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarTelemetria([FromBody] CadastrarTelemetria dados)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var resultado = await _service.CadastrarTelemetria(dados);

            if (resultado == null)
                return NotFound(new { mensagem = "Não foi possível registrar a telemetria, turista não encontrado na base." });

            return CreatedAtAction(nameof(ListarHistorico), new { turistaId = resultado.TuristaId }, resultado);
        }

        [HttpGet("turista/{turistaId:int}")]
        public async Task<IActionResult> ListarHistorico(int turistaId)
        {
            var historico = await _service.ListarHistoricoPorTurista(turistaId);
            if (historico == null)
                return NotFound(new{ mensagem = "Histórico indisponível, o turista informado não existe." });

            return Ok(historico);
        }
    }
}
