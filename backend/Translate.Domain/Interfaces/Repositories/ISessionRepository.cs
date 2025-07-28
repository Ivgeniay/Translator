using Translate.Domain.Entities;

namespace Translate.Domain.Interfaces.Repositories
{
    public interface ISessionRepository
    {
        Task<TranslationSession?> GetByIdAsync(string id);
        Task<TranslationSession?> GetByConnectionIdAsync(string connectionId);
        Task<List<TranslationSession>> GetActiveSessionsAsync();
        Task<List<TranslationSession>> GetExpiredSessionsAsync(TimeSpan timeout);
        Task<List<TranslationSession>> GetSessionsByUserIdAsync(string userId);
        Task AddAsync(TranslationSession session);
        Task UpdateAsync(TranslationSession session);
        Task DeleteAsync(string id);
    }
}