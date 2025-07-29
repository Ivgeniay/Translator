using Translator.Domain.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Translator.CommonLib.Models;
using Translator.CommonLib.Events;
using CommonLib.Models.Responses;

namespace Translator.Application.Services
{
    public class TranslationHub : Hub
    {
        private readonly ILogger<TranslationHub> _logger;
        private readonly ITranslationOrchestrator _orchestrator;

        public TranslationHub(
            ILogger<TranslationHub> logger,
            ITranslationOrchestrator orchestrator)
        {
            _logger = logger;
            _orchestrator = orchestrator;
        }

        public async Task JoinSession(string? userId = null)
        {
            try
            {
                var session = await _orchestrator.CreateSessionAsync(Context.ConnectionId, userId);

                await Clients.Caller.SendAsync(SignalREvents.SessionCreated, new SessionCreatedResponse
                {
                    SessionId = session.Id,
                    ConnectionId = Context.ConnectionId
                });

                _logger.LogInformation("Создана сессия {SessionId} для клиента {ConnectionId}",
                    session.Id, Context.ConnectionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании сессии для клиента {ConnectionId}", Context.ConnectionId);

                await Clients.Caller.SendAsync(SignalREvents.Error, new ErrorResponse
                {
                    Message = "Ошибка при создании сессии",
                    Details = ex.Message
                });
            }
        }

        public async Task StartTranslation(TranslationRequest request)
        {
            try
            {
                var session = await _orchestrator.GetSessionByConnectionIdAsync(Context.ConnectionId);
                if (session == null)
                {
                    await Clients.Caller.SendAsync(SignalREvents.Error, new ErrorResponse
                    {
                        Message = "Сессия не найдена. Выполните JoinSession сначала."
                    });
                    return;
                }

                var job = await _orchestrator.ProcessTranslationAsync(session.Id, request);

                await Clients.Caller.SendAsync(SignalREvents.TranslationStarted, new TranslationStartedResponse
                {
                    JobId = job.Id,
                    Status = job.Status.ToString()
                });

                _logger.LogInformation("Начата обработка перевода {JobId} для сессии {SessionId}",
                    job.Id, session.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запуске перевода для клиента {ConnectionId}", Context.ConnectionId);

                await Clients.Caller.SendAsync(SignalREvents.TranslationFailed, new TranslationFailedResponse
                {
                    Error = ex.Message,
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        public async Task GetJobStatus(string jobId)
        {
            try
            {
                var result = await _orchestrator.GetJobResultAsync(jobId);

                await Clients.Caller.SendAsync(SignalREvents.JobStatus, new JobStatusResponse
                {
                    JobId = jobId,
                    Status = "Completed",
                    Result = result
                });
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync(SignalREvents.JobStatus, new JobStatusResponse
                {
                    JobId = jobId,
                    Status = "Failed",
                    Error = ex.Message
                });
            }
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation("Клиент подключился: {ConnectionId}", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                var session = await _orchestrator.GetSessionByConnectionIdAsync(Context.ConnectionId);
                if (session != null)
                {
                    await _orchestrator.CloseSessionAsync(session.Id);
                    _logger.LogInformation("Сессия {SessionId} закрыта при отключении клиента {ConnectionId}",
                        session.Id, Context.ConnectionId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при закрытии сессии для отключившегося клиента {ConnectionId}", Context.ConnectionId);
            }

            _logger.LogInformation("Клиент отключился: {ConnectionId}", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}