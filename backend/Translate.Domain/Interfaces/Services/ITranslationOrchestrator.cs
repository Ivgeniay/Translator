using Translate.Domain.Entities;
using CommonLib.Models;

namespace Translate.Domain.Interfaces.Services
{
    public interface ITranslationOrchestrator
    {
        Task<TranslationSession> CreateSessionAsync(string connectionId, string? userId = null);
        Task<TranslationJob> ProcessTranslationAsync(string sessionId, TranslationRequest request);
        Task<TranslationResponse> GetJobResultAsync(string jobId);
        Task CloseSessionAsync(string sessionId);
        Task<TranslationSession?> GetSessionByConnectionIdAsync(string connectionId);
        Task<List<TranslationSession>> GetActiveSessionsAsync();
        Task CleanupExpiredSessionsAsync(TimeSpan timeout);
    }
}