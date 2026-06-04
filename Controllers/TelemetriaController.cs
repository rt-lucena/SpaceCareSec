using Microsoft.AspNetCore.Mvc;
using SpaceCare.Domain.Telemetrias.Dtos;
using SpaceCare.Services;

namespace SpaceCare.Controllers
{
    /// <summary>
    /// Controller responsável por expor os endpoints relacionados à gestão das telemetrias dos Turistas Espaciais.
    /// </summary>
    [ApiController]
    [Route("telemetrias")]
    public class TelemetriaController : ControllerBase
    {
        private readonly TelemetriaService _service;

        /// <summary>
        /// Inicializa uma nova instância do <see cref="TelemetriaController"/>.
        /// </summary>
        /// <param name="service">O serviço de domínio de telemetrias injetado.</param>
        public TelemetriaController(TelemetriaService service)
        {
            _service = service;
        }

        /// <summary>
        /// Endpoint para cadastrar uma nova telemetria de um turista espacial.
        /// </summary>
        /// <param name="dados">Contrato contendo as aferições de batimentos, temperatura e pressão arterial.</param>
        /// <returns>O objeto detalhado da telemetria persistido no banco.</returns>
        /// <response code="201">Retornado quando a telemetria é gravada com sucesso.</response>
        /// <response code="400">Retornado se houver falha de validação no modelo ou formato inválido da pressão arterial.</response>
        /// <response code="404">Retornado pelo Middleware caso o turista vinculado não exista.</response>
        [HttpPost]
        public async Task<IActionResult> CadastrarTelemetria([FromBody] CadastrarTelemetria dados)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var resultado = await _service.CadastrarTelemetria(dados);

            return CreatedAtAction(nameof(ListarHistorico), new { turistaId = resultado.TuristaId }, resultado);
        }

        /// <summary>
        /// Endpoint para listar o histórico completo de telemetrias de um turista espacial específico.
        /// </summary>
        /// <param name="turistaId">O identificador numérico único do turista.</param>
        /// <returns>Uma lista contendo todas as telemetrias do usuário ordenadas por leitura recente.</returns>
        /// <response code="200">Operação executada com sucesso.</response>
        /// <response code="404">Retornado pelo Middleware caso o turista não exista ou seja removido.</response>
        [HttpGet("turista/{turistaId:int}")]
        public async Task<IActionResult> ListarHistorico(int turistaId)
        {
            var historico = await _service.ListarHistoricoPorTurista(turistaId);
            return Ok(historico);
        }

        /// <summary>
        /// Endpoint para listar todas as telemetrias registradas no sistema.
        /// </summary>
        /// <returns>Uma lista contendo todas as telemetrias ordenadas por leitura recente.</returns>
        /// <response code="200">Operação executada com sucesso.</response>
        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            var telemetrias = await _service.ListarTodasTelemetrias();
            return Ok(telemetrias);
        }
    }
}
