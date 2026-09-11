using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Application.Contracts.Services;
using Application.Contracts.Services.Chatbot;
using Application.DTOs.Chats.Chat;

namespace Infrastructure.ExternalServices
{
    /// <summary>
    /// Cliente HTTP hacia el chatbot de FastAPI (NexusOdonto_ChatBot_AI).
    /// </summary>
    public class ChatbotHttpClient : IChatbotClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ChatbotHttpClient> _logger;
        private readonly TimeSpan _timeout;
        private readonly string _messagePath;
        private readonly string _internalSecret;

        public ChatbotHttpClient(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<ChatbotHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            var baseUrl = configuration["Chatbot:BaseUrl"]
                ?? configuration["CHATBOT_BASE_URL"]
                ?? "http://localhost:8000";
            _httpClient.BaseAddress = new Uri(baseUrl);

            _internalSecret = configuration["Chatbot:InternalSecret"]
                ?? configuration["AGENT_INTERNAL_SECRET"]
                ?? "TOKEN_SECRETO_INTERNO_NET_2026";

            var path = configuration["Chatbot:MessagePath"]
                ?? configuration["CHATBOT_MESSAGE_PATH"]
                ?? "/api/v1/agent/send-message";
            _messagePath = path.StartsWith('/') ? path : "/" + path;

            var timeoutSeconds = 30;
            var timeoutStr = configuration["Chatbot:TimeoutSeconds"]
                ?? configuration["CHATBOT_TIMEOUT_SECONDS"];
            if (int.TryParse(timeoutStr, out var configured) && configured > 0)
                timeoutSeconds = configured;
            _timeout = TimeSpan.FromSeconds(timeoutSeconds);
        }

        public async Task<ChatBotResponseDto> SendMessageAsync(string sessionId, string message)
        {
            try
            {
                using var cts = new CancellationTokenSource(_timeout);
                var payload = new { session_id = sessionId, message = message };
                var request = new HttpRequestMessage(HttpMethod.Post, _messagePath)
                {
                    Content = JsonContent.Create(payload)
                };
                if (!string.IsNullOrWhiteSpace(_internalSecret))
                {
                    request.Headers.Add("X-Internal-Secret", _internalSecret);
                }

                var response = await _httpClient.SendAsync(request, cts.Token);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Chatbot devolvio HTTP status {StatusCode} en {Path}", response.StatusCode, _messagePath);
                    return new ChatBotResponseDto { Response = "Error de comunicaci�n con chatbot", State = "ERROR" };
                }

                return new ChatBotResponseDto { Response = "OK", State = "COMPLETED" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error llamando al chatbot");
                return new ChatBotResponseDto { Response = "Error de comunicaci�n", State = "ERROR" };
            }
        }

        public async Task<bool> SendAgentMessageAsync(string phoneNumber, string message, CancellationToken cancellationToken = default)
        {
            try
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(_timeout);

                var payload = new
                {
                    phone_number = phoneNumber,
                    message = message,
                    sender_role = "AGENTE_HUMANO",
                    skip_db_register = true
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/agent/send-message")
                {
                    Content = JsonContent.Create(payload)
                };

                if (!string.IsNullOrWhiteSpace(_internalSecret))
                {
                    request.Headers.Add("X-Internal-Secret", _internalSecret);
                }

                var response = await _httpClient.SendAsync(request, cts.Token);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Mensaje de agente humano enviado exitosamente a FastAPI para {Phone}", phoneNumber);
                    return true;
                }

                var errText = await response.Content.ReadAsStringAsync(cts.Token);
                _logger.LogWarning("No se pudo enviar mensaje de agente humano a FastAPI (HTTP {Status}): {Err}", response.StatusCode, errText);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando mensaje de agente humano hacia FastAPI para {Phone}", phoneNumber);
                return false;
            }
        }

        public async Task<bool> ResumeConversationAsync(string phoneNumber, bool clearHistory = false, CancellationToken cancellationToken = default)
        {
            try
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(_timeout);

                var payload = new
                {
                    phone_number = phoneNumber,
                    clear_history = clearHistory
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/conversations/resume")
                {
                    Content = JsonContent.Create(payload)
                };

                if (!string.IsNullOrWhiteSpace(_internalSecret))
                {
                    request.Headers.Add("X-Internal-Secret", _internalSecret);
                }

                var response = await _httpClient.SendAsync(request, cts.Token);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Conversaci�n reactivada exitosamente en FastAPI para {Phone}", phoneNumber);
                    return true;
                }

                var errText = await response.Content.ReadAsStringAsync(cts.Token);
                _logger.LogWarning("No se pudo reactivar conversaci�n en FastAPI (HTTP {Status}): {Err}", response.StatusCode, errText);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al notificar reanudaci�n de conversaci�n a FastAPI para {Phone}", phoneNumber);
                return false;
            }
        }
    }
}
