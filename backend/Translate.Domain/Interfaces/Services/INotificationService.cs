using Translate.Domain.Entities;
using CommonLib.Models;

namespace Translate.Domain.Interfaces.Services
{
    public interface INotificationService
    {
        Task NotifyTranslationCompletedAsync(string connectionId, TranslationResponse response);
        Task NotifyTranslationFailedAsync(string connectionId, string jobId, string errorMessage);
        Task NotifySessionClosedAsync(string connectionId);
        Task BroadcastSystemMessageAsync(string message);
        Task NotifyJobStatusChangedAsync(string connectionId, string jobId, TranslationJobStatus status);
    }
}