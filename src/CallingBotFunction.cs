using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TeamsCallingBot.Models;
using TeamsCallingBot.Services;

namespace TeamsCallingBot
{
    public class CallingBotFunction
    {
        private readonly GraphApiService _graphApiService;
        private readonly TranscriptionService _transcriptionService;
        private readonly DataService _dataService;

        public CallingBotFunction(
            GraphApiService graphApiService,
            TranscriptionService transcriptionService,
            DataService dataService)
        {
            _graphApiService = graphApiService;
            _transcriptionService = transcriptionService;
            _dataService = dataService;
        }

        /// <summary>
        /// Inicia una llamada saliente a un usuario de Teams
        /// Endpoint: POST /api/call/outbound
        /// </summary>
        [FunctionName("InitiateOutboundCall")]
        public async Task<IActionResult> InitiateOutboundCall(
            [HttpTrigger(
                Microsoft.Azure.WebJobs.HttpMethod.Post,
                Route = "call/outbound")]
            HttpRequestMessage req,
            ILogger log)
        {
            try
            {
                log.LogInformation("Iniciando llamada saliente");

                var requestBody = await req.Content.ReadAsAsync<CallRequest>();

                if (requestBody == null)
                    return new BadRequestObjectResult("El cuerpo de la solicitud es requerido");

                // Validar campos requeridos
                if (string.IsNullOrEmpty(requestBody.UserId))
                    return new BadRequestObjectResult("UserId es requerido");

                if (requestBody.Questions == null || requestBody.Questions.Count == 0)
                    return new BadRequestObjectResult("Al menos una pregunta es requerida");

                // Iniciar la llamada
                var callResponse = await _graphApiService.InitiateOutboundCallAsync(requestBody);

                if (callResponse.Status == "failed")
                {
                    log.LogError($"Error al iniciar llamada: {callResponse.ErrorMessage}");
                    return new ObjectResult(callResponse) { StatusCode = 500 };
                }

                // Guardar registro de la llamada
                await _dataService.SaveCallRecordAsync(callResponse);

                log.LogInformation($"Llamada iniciada: {callResponse.CallId}");

                return new OkObjectResult(callResponse);
            }
            catch (Exception ex)
            {
                log.LogError($"Excepción: {ex.Message}");
                return new ObjectResult(
                    new { error = ex.Message }
                ) { StatusCode = 500 };
            }
        }

        /// <summary>
        /// Obtiene el estado de una llamada
        /// Endpoint: GET /api/call/{callId}/status
        /// </summary>
        [FunctionName("GetCallStatus")]
        public async Task<IActionResult> GetCallStatus(
            [HttpTrigger(
                Microsoft.Azure.WebJobs.HttpMethod.Get,
                Route = "call/{callId}/status")]
            HttpRequestMessage req,
            string callId,
            ILogger log)
        {
            try
            {
                log.LogInformation($"Obteniendo estado de llamada: {callId}");

                if (string.IsNullOrEmpty(callId))
                    return new BadRequestObjectResult("callId es requerido");

                var status = await _graphApiService.GetCallStatusAsync(callId);
                return new OkObjectResult(status);
            }
            catch (Exception ex)
            {
                log.LogError($"Excepción: {ex.Message}");
                return new ObjectResult(
                    new { error = ex.Message }
                ) { StatusCode = 500 };
            }
        }

        /// <summary>
        /// Finaliza una llamada
        /// Endpoint: POST /api/call/{callId}/end
        /// </summary>
        [FunctionName("EndCall")]
        public async Task<IActionResult> EndCall(
            [HttpTrigger(
                Microsoft.Azure.WebJobs.HttpMethod.Post,
                Route = "call/{callId}/end")]
            HttpRequestMessage req,
            string callId,
            ILogger log)
        {
            try
            {
                log.LogInformation($"Finalizando llamada: {callId}");

                if (string.IsNullOrEmpty(callId))
                    return new BadRequestObjectResult("callId es requerido");

                var success = await _graphApiService.EndCallAsync(callId);

                if (!success)
                    return new ObjectResult(
                        new { error = "No se pudo finalizar la llamada" }
                    ) { StatusCode = 500 };

                return new OkObjectResult(new { message = "Llamada finalizada exitosamente" });
            }
            catch (Exception ex)
            {
                log.LogError($"Excepción: {ex.Message}");
                return new ObjectResult(
                    new { error = ex.Message }
                ) { StatusCode = 500 };
            }
        }

        /// <summary>
        /// Procesa el callback de la llamada (webhooks de Graph API)
        /// Endpoint: POST /api/call/webhook
        /// </summary>
        [FunctionName("CallWebhook")]
        public async Task<IActionResult> CallWebhook(
            [HttpTrigger(
                Microsoft.Azure.WebJobs.HttpMethod.Post,
                Route = "call/webhook")]
            HttpRequestMessage req,
            ILogger log)
        {
            try
            {
                log.LogInformation("Webhook de llamada recibido");

                var requestBody = await req.Content.ReadAsStringAsync();
                log.LogInformation($"Payload: {requestBody}");

                // Procesar el webhook según el tipo de evento
                // Aquí iría la lógica para manejar eventos de llamada:
                // - Call started
                // - Call ended
                // - DTMF received
                // - etc.

                return new OkObjectResult(new { message = "Webhook procesado" });
            }
            catch (Exception ex)
            {
                log.LogError($"Excepción: {ex.Message}");
                return new ObjectResult(
                    new { error = ex.Message }
                ) { StatusCode = 500 };
            }
        }
    }
}
