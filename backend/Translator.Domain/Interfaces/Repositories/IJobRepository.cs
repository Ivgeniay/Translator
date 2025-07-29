using Translator.Domain.Entities;

namespace Translator.Domain.Interfaces.Repositories
{
    public interface IJobRepository
    {
        Task<TranslationJob?> GetByIdAsync(string id);
        Task<List<TranslationJob>> GetBySessionIdAsync(string sessionId);
        Task<List<TranslationJob>> GetPendingJobsAsync();
        Task<List<TranslationJob>> GetFailedJobsAsync();
        Task<List<TranslationJob>> GetJobsByStatusAsync(TranslationJobStatus status);
        Task AddAsync(TranslationJob job);
        Task UpdateAsync(TranslationJob job);
        Task DeleteAsync(string id);
        Task<int> GetActiveJobsCountAsync();
    }
}