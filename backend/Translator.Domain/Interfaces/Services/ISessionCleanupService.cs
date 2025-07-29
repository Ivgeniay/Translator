namespace Translator.Domain.Interfaces.Services
{
    public interface ISessionCleanupService
    {
        Task CleanupExpiredSessionsAsync();
        Task CleanupCompletedJobsAsync(TimeSpan retentionPeriod);
        Task CleanupUserHistoryAsync(int maxHistoryItems);
        Task PerformDailyCleanupAsync();
    }
}