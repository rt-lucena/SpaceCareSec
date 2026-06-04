using SpaceCare.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace SpaceCare.Infra.MIddleware
{
    /// <summary>
    /// Middleware global para interceptação, tratamento e padronização de exceções da API.
    /// </summary>
    public class TratadorGlobalErros 
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="TratadorGlobalErros"/>.
        /// </summary>
        /// <param name="next">O próximo delegado de requisição HTTP a ser executado no fluxo.</param>
        public TratadorGlobalErros(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invoca a requisição HTTP permitindo capturar qualquer exceção disparada no fluxo dos Services.
        /// </summary>
        /// <param name="context">O contexto HTTP da requisição atual (<see cref="HttpContext"/>).</param>
        /// <returns>Uma <see cref="Task"/> que representa a operação assíncrona de processamento da requisição.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await TratarExcecaoAsync(context, ex);
            }
        }

        /// <summary>
        /// Trata a exceção capturada, mapeando-a para um status code HTTP apropriado e retornando uma resposta JSON padronizada.
        /// </summary>
        /// <param name="context">O contexto HTTP onde a resposta de erro será gravada.</param>
        /// <param name="excecao">A exceção disparada que disparou o tratamento de erro.</param>
        /// <returns>Uma <see cref="Task"/> que representa a operação assíncrona de tratamento da exceção.</returns>
        private static Task TratarExcecaoAsync(HttpContext context, Exception excecao)
        {
            context.Response.ContentType = "application/json";

            var statusCode = HttpStatusCode.InternalServerError;
            string mensagemResponse = "Ocorreu um erro inesperado no servidor espacial.";

            if (excecao is TuristaNotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                mensagemResponse = excecao.Message;
            }
            else if (excecao is ArgumentException)
            {
                statusCode = HttpStatusCode.BadRequest;
                mensagemResponse = excecao.Message;
            }

            context.Response.StatusCode = (int)statusCode;

            var payload = new { mensagem = mensagemResponse };
            var jsonString = JsonSerializer.Serialize(payload);

            return context.Response.WriteAsync(jsonString);
        }
    }
}
