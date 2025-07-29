using Translator.Domain.Entities;
using Translator.Domain.Interfaces.Repositories;

namespace Translator.Persistence.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        public Task AddAsync(TranslationSession session)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<TranslationSession>> GetActiveSessionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TranslationSession?> GetByConnectionIdAsync(string connectionId)
        {
            throw new NotImplementedException();
        }

        public Task<TranslationSession?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<TranslationSession>> GetExpiredSessionsAsync(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public Task<List<TranslationSession>> GetSessionsByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TranslationSession session)
        {
            throw new NotImplementedException();
        }
    }
}