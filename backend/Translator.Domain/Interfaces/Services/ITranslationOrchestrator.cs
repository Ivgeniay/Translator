using Translator.Domain.Entities;
using Translator.CommonLib.Models;

namespace Translator.Domain.Interfaces.Services
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