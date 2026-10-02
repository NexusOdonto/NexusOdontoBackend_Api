using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Application.Contracts.Services.Chatbot;
using Application.DTOs.Chatbot.Gateway;
using Application.DTOs.Chats.Chat;
using Application.DTOs.Chats.ScheduleChange;

namespace Infrastructure.ExternalServices;

/// <summary>
/// Cliente HTTP hacia el chatbot de FastAPI (NexusOdonto_ChatBot_AI).
/// </summary>
public class ChatbotHttpClient : IChatbotClient
{
        private static readonly TimeSpan ScheduleChangeTimeout = TimeSpan.FromSeconds(20);

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
                    return new ChatBotResponseDto { Response = "Error de comunicacion con chatbot", State = "ERROR" };
                }

                return new ChatBotResponseDto { Response = "OK", State = "COMPLETED" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error llamando al chatbot");
                return new ChatBotResponseDto { Response = "Error de comunicacion", State = "ERROR" };
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

        public async Task<ScheduleChangeNotifyResult> NotifyScheduleChangeAsync(
            ScheduleChangeNotificationDto notification,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(notification);

            try
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(ScheduleChangeTimeout);

                var payload = new
                {
                    appointment_id = notification.AppointmentId,
                    patient_id = notification.PatientId,
                    patient_name = notification.PatientName,
                    phone = notification.Phone,
                    service_id = notification.ServiceId,
                    service = notification.Service,
                    professional_id = notification.ProfessionalId,
                    professional = notification.Professional,
                    starts_at = notification.StartsAt.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                    ends_at = notification.EndsAt.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)
                };

                using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/agent/schedule-change")
                {
                    Content = JsonContent.Create(payload)
                };

                if (!string.IsNullOrWhiteSpace(_internalSecret))
                {
                    request.Headers.Add("X-Internal-Secret", _internalSecret);
                }

                using var response = await _httpClient.SendAsync(request, cts.Token);
                var body = await response.Content.ReadAsStringAsync(cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "Chatbot rechazo aviso de cambio de horario para cita {AppointmentId} (HTTP {Status})",
                        notification.AppointmentId, (int)response.StatusCode);
                    return new ScheduleChangeNotifyResult
                    {
                        Success = false,
                        Error = Truncate($"HTTP {(int)response.StatusCode}: {body}", 900)
                    };
                }

                string? status = null;
                try
                {
                    using var doc = JsonDocument.Parse(string.IsNullOrWhiteSpace(body) ? "{}" : body);
                    if (doc.RootElement.ValueKind == JsonValueKind.Object &&
                        doc.RootElement.TryGetProperty("status", out var statusProp) &&
                        statusProp.ValueKind == JsonValueKind.String)
                    {
                        status = statusProp.GetString();
                    }
                }
                catch (JsonException)
                {
                    // Non-JSON 2xx body: treat as sent.
                }

                status = string.IsNullOrWhiteSpace(status) ? "sent" : status.Trim().ToLowerInvariant();
                if (status is not ("sent" or "queued"))
                {
                    return new ScheduleChangeNotifyResult
                    {
                        Success = false,
                        Status = status,
                        Error = Truncate($"Estado inesperado del chatbot: {body}", 900)
                    };
                }

                return new ScheduleChangeNotifyResult { Success = true, Status = status };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando aviso de cambio de horario al chatbot para cita {AppointmentId}", notification.AppointmentId);
                return new ScheduleChangeNotifyResult
                {
                    Success = false,
                    Error = Truncate(ex is OperationCanceledException ? "Timeout llamando al chatbot" : ex.Message, 900)
                };
            }
        }

        private static string Truncate(string value, int max) =>
            value.Length <= max ? value : value[..max];

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
                    _logger.LogInformation("Conversacion reactivada exitosamente en FastAPI para {Phone}", phoneNumber);
                    return true;
                }

                var errText = await response.Content.ReadAsStringAsync(cts.Token);
                _logger.LogWarning("No se pudo reactivar conversacion en FastAPI (HTTP {Status}): {Err}", response.StatusCode, errText);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al notificar reanudacion de conversacion a FastAPI para {Phone}", phoneNumber);
                return false;
            }
        }

        public async Task<bool> ResumeAllConversationsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(_timeout);

                var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/conversations/resume-all");
                if (!string.IsNullOrWhiteSpace(_internalSecret))
                {
                    request.Headers.Add("X-Internal-Secret", _internalSecret);
                }

                var response = await _httpClient.SendAsync(request, cts.Token);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error llamando a resume-all en FastAPI");
                return false;
            }
        }

        public async Task<QrDataResponseDto?> GetQrDataAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(TimeSpan.FromSeconds(10));

                var request = new HttpRequestMessage(HttpMethod.Get, "/qr/data");
                var response = await _httpClient.SendAsync(request, cts.Token);
                if (!response.IsSuccessStatusCode)
                {
                    return new QrDataResponseDto { Connected = false, State = "error" };
                }

                return await response.Content.ReadFromJsonAsync<QrDataResponseDto>(cancellationToken: cts.Token);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error obteniendo datos QR de FastAPI");
                return new QrDataResponseDto { Connected = false, State = "error" };
            }
        }

        public async Task<bool> RestartQrAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(TimeSpan.FromSeconds(15));

                var request = new HttpRequestMessage(HttpMethod.Post, "/qr/restart");
                var response = await _httpClient.SendAsync(request, cts.Token);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al reiniciar instancia QR en FastAPI");
                return false;
            }
        }

        public async Task<bool> LogoutQrAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(TimeSpan.FromSeconds(15));

                var request = new HttpRequestMessage(HttpMethod.Post, "/qr/logout");
                var response = await _httpClient.SendAsync(request, cts.Token);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cerrar sesión QR en FastAPI");
                return false;
            }
        }

        public async Task<ResolveIdentityResponseDto?> ResolveIdentityAsync(string identifier, Guid? conversationId = null, CancellationToken cancellationToken = default)
        {
            try
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(TimeSpan.FromSeconds(10));

                var payload = new
                {
                    identifier = identifier,
                    conversation_id = conversationId?.ToString()
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/whatsapp/resolve-identity")
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
                    return new ResolveIdentityResponseDto { Status = "unresolved", Phone = null, Identifier = identifier };
                }

                return await response.Content.ReadFromJsonAsync<ResolveIdentityResponseDto>(cancellationToken: cts.Token);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error al resolver identidad en FastAPI para {Ident}", identifier);
                return new ResolveIdentityResponseDto { Status = "unresolved", Phone = null, Identifier = identifier };
            }
        }
    }
