using Translate.Domain.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Translate.Domain.Entities;
using CommonLib.Models;

namespace Translate.Application.Services
{

    public class NotificationService : INotificationService
    {
        private readonly IHubContext<TranslationHub> _hubContext;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            IHubContext<TranslationHub> hubContext,
            ILogger<NotificationService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task NotifyTranslationCompletedAsync(string connectionId, TranslationResponse response)
        {
            try
            {
                await _hubContext.Clients.Client(connectionId)
                    .SendAsync("TranslationCompleted", response);

                _logger.LogInformation(
                    "Уведомление о завершении перевода отправлено клиенту {ConnectionId}",
                    connectionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Ошибка при отправке уведомления о завершении перевода клиенту {ConnectionId}",
                    connectionId);
            }
        }

        public async Task NotifyTranslationFailedAsync(string connectionId, string jobId, string errorMessage)
        {
            try
            {
                var errorResponse = new
                {
                    JobId = jobId,
                    Error = errorMessage,
                    Timestamp = DateTime.UtcNow
                };

                await _hubContext.Clients.Client(connectionId)
                    .SendAsync("TranslationFailed", errorResponse);

                _logger.LogInformation(
                    "Уведомление об ошибке перевода отправлено клиенту {ConnectionId}",
                    connectionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Ошибка при отправке уведомления об ошибке перевода клиенту {ConnectionId}",
                    connectionId);
            }
        }

        public async Task NotifySessionClosedAsync(string connectionId)
        {
            try
            {
                await _hubContext.Clients.Client(connectionId)
                    .SendAsync("SessionClosed");

                _logger.LogInformation(
                    "Уведомление о закрытии сессии отправлено клиенту {ConnectionId}",
                    connectionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Ошибка при отправке уведомления о закрытии сессии клиенту {ConnectionId}",
                    connectionId);
            }
        }

        public async Task BroadcastSystemMessageAsync(string message)
        {
            try
            {
                var systemMessage = new
                {
                    Message = message,
                    Type = "System",
                    Timestamp = DateTime.UtcNow
                };

                await _hubContext.Clients.All
                    .SendAsync("SystemMessage", systemMessage);

                _logger.LogInformation("Системное сообщение отправлено всем клиентам: {Message}", message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при отправке системного сообщения: {Message}", message);
            }
        }

        public async Task NotifyJobStatusChangedAsync(string connectionId, string jobId, TranslationJobStatus status)
        {
            try
            {
                var statusUpdate = new
                {
                    JobId = jobId,
                    Status = status.ToString(),
                    Timestamp = DateTime.UtcNow
                };

                await _hubContext.Clients.Client(connectionId)
                    .SendAsync("JobStatusChanged", statusUpdate);

                _logger.LogInformation(
                    "Уведомление об изменении статуса задачи {JobId} отправлено клиенту {ConnectionId}",
                    jobId, connectionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Ошибка при отправке уведомления о статусе задачи {JobId} клиенту {ConnectionId}",
                    jobId, connectionId);
            }
        }
    }
}