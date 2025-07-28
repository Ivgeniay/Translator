using Translate.Domain.Entities;
using Translate.Domain.Interfaces.Repositories;

namespace Translate.Persistence.Repositories
{
    public class JobRepository : IJobRepository
    {
        public Task AddAsync(TranslationJob job)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetActiveJobsCountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TranslationJob?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<TranslationJob>> GetBySessionIdAsync(string sessionId)
        {
            throw new NotImplementedException();
        }

        public Task<List<TranslationJob>> GetFailedJobsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<TranslationJob>> GetJobsByStatusAsync(TranslationJobStatus status)
        {
            throw new NotImplementedException();
        }

        public Task<List<TranslationJob>> GetPendingJobsAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TranslationJob job)
        {
            throw new NotImplementedException();
        }
    }
}