using Translator.Domain.Entities;

namespace Translator.Domain.Interfaces.Services
{
    public interface ITranslationJobProcessor
    {
        Task EnqueueAsync(string jobId);
        Task<TranslationJob?> DequeueAsync();
        Task ProcessJobAsync(TranslationJob job);
        Task RetryFailedJobsAsync();
        Task<int> GetQueueSizeAsync();
        Task<List<string>> GetPendingJobIdsAsync();
        Task CancelJobAsync(string jobId);
        Task ClearQueueAsync();
    }
}