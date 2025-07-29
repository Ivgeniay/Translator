using Translator.Domain.Entities;
using Translator.CommonLib.Models;

namespace Translator.Domain.Interfaces.Services
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