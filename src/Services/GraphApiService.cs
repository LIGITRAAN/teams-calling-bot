using System;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using TeamsCallingBot.Models;
using Azure.Identity;

namespace TeamsCallingBot.Services
{
    public class GraphApiService
    {
        private readonly GraphServiceClient _graphClient;
        private readonly string _tenantId;
        private readonly string _clientId;
        private readonly string _clientSecret;

        public GraphApiService(string tenantId, string clientId, string clientSecret)
        {
            _tenantId = tenantId;
            _clientId = clientId;
            _clientSecret = clientSecret;

            var credential = new ClientSecretCredential(_tenantId, _clientId, _clientSecret);
            _graphClient = new GraphServiceClient(credential);
        }

        /// <summary>
        /// Inicia una llamada saliente a un usuario de Teams
        /// </summary>
        public async Task<CallResponse> InitiateOutboundCallAsync(CallRequest request)
        {
            try
            {
                // Validar entrada
                if (string.IsNullOrEmpty(request.UserId))
                    throw new ArgumentException("UserId es requerido");

                // Crear la solicitud de llamada
                var callBody = new Call
                {
                    CallbackUri = request.CallbackUrl,
                    Targets = new System.Collections.Generic.List<InvitationParticipantInfo>
                    {
                        new InvitationParticipantInfo
                        {
                            Identity = new IdentitySet
                            {
                                User = new Identity
                                {
                                    DisplayName = request.UserId,
                                    Id = request.UserId
                                }
                            }
                        }
                    },
                    RequestedModalities = new System.Collections.Generic.List<Modality?>
                    {
                        Modality.Audio
                    },
                    MediaConfig = new ServiceHostedMediaConfig()
                };

                // Realizar la llamada
                var call = await _graphClient.Communications.Calls
                    .Request()
                    .AddAsync(callBody);

                return new CallResponse
                {
                    CallId = call.Id,
                    Status = call.State.ToString(),
                    StartTime = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                return new CallResponse
                {
                    Status = "failed",
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Obtiene el estado actual de una llamada
        /// </summary>
        public async Task<CallResponse> GetCallStatusAsync(string callId)
        {
            try
            {
                var call = await _graphClient.Communications.Calls[callId]
                    .Request()
                    .GetAsync();

                return new CallResponse
                {
                    CallId = call.Id,
                    Status = call.State.ToString(),
                    Duration = call.DurationInSeconds ?? 0
                };
            }
            catch (Exception ex)
            {
                return new CallResponse
                {
                    Status = "error",
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Finaliza una llamada
        /// </summary>
        public async Task<bool> EndCallAsync(string callId)
        {
            try
            {
                await _graphClient.Communications.Calls[callId]
                    .Request()
                    .DeleteAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
