using Translator.Domain.Interfaces.Repositories;
using Translator.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Translator.Domain.Entities;

namespace Translator.Application.Services
{
    public class SessionCleanupService : ISessionCleanupService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<SessionCleanupService> _logger;
        private readonly TimeSpan _defaultSessionTimeout = TimeSpan.FromHours(1);
        private readonly TimeSpan _defaultJobRetention = TimeSpan.FromDays(7);
        private readonly int _defaultMaxHistoryItems = 100;

        public SessionCleanupService(
            ISessionRepository sessionRepository,
            IJobRepository jobRepository,
            IUserRepository userRepository,
            ILogger<SessionCleanupService> logger)
        {
            _sessionRepository = sessionRepository;
            _jobRepository = jobRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task CleanupExpiredSessionsAsync()
        {
            try
            {
                var expiredSessions = await _sessionRepository.GetExpiredSessionsAsync(_defaultSessionTimeout);
                var cleanedCount = 0;

                foreach (var session in expiredSessions)
                {
                    if (session.Status == TranslationSessionStatus.Active)
                    {
                        session.Close();
                        await _sessionRepository.UpdateAsync(session);
                        cleanedCount++;
                    }
                }

                _logger.LogInformation("Очищено истекших сессий: {Count}", cleanedCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при очистке истекших сессий");
            }
        }

        public async Task CleanupCompletedJobsAsync(TimeSpan retentionPeriod)
        {
            try
            {
                var completedJobs = await _jobRepository.GetJobsByStatusAsync(TranslationJobStatus.Completed);
                var cutoffDate = DateTime.UtcNow - retentionPeriod;
                var cleanedCount = 0;

                foreach (var job in completedJobs.Where(j => j.CompletedAt < cutoffDate))
                {
                    await _jobRepository.DeleteAsync(job.Id);
                    cleanedCount++;
                }

                var failedJobs = await _jobRepository.GetJobsByStatusAsync(TranslationJobStatus.Failed);
                foreach (var job in failedJobs.Where(j => j.CompletedAt < cutoffDate && !j.CanRetry(3)))
                {
                    await _jobRepository.DeleteAsync(job.Id);
                    cleanedCount++;
                }

                _logger.LogInformation("Удалено старых задач: {Count}", cleanedCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при очистке завершенных задач");
            }
        }

        public async Task CleanupUserHistoryAsync(int maxHistoryItems)
        {
            try
            {
                var users = await _userRepository.GetActiveUsersAsync();
                var cleanedUsersCount = 0;

                foreach (var user in users)
                {
                    if (user.SessionHistory.Count > maxHistoryItems)
                    {
                        var itemsToRemove = user.SessionHistory.Count - maxHistoryItems;
                        user.SessionHistory.RemoveRange(0, itemsToRemove);
                        await _userRepository.UpdateAsync(user);
                        cleanedUsersCount++;
                    }
                }

                _logger.LogInformation("Очищена история пользователей: {Count}", cleanedUsersCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при очистке истории пользователей");
            }
        }

        public async Task PerformDailyCleanupAsync()
        {
            _logger.LogInformation("Начинается ежедневная очистка системы");

            try
            {
                await CleanupExpiredSessionsAsync();
                await CleanupCompletedJobsAsync(_defaultJobRetention);
                await CleanupUserHistoryAsync(_defaultMaxHistoryItems);

                await CleanupInactiveSessions();
                await CleanupCancelledJobs();

                _logger.LogInformation("Ежедневная очистка системы завершена успешно");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении ежедневной очистки");
            }
        }

        private async Task CleanupInactiveSessions()
        {
            try
            {
                var allSessions = await _sessionRepository.GetActiveSessionsAsync();
                var inactiveTimeout = TimeSpan.FromHours(24);
                var cleanedCount = 0;

                foreach (var session in allSessions.Where(s => s.IsExpired(inactiveTimeout)))
                {
                    session.Close();
                    await _sessionRepository.UpdateAsync(session);
                    cleanedCount++;
                }

                _logger.LogInformation("Закрыто неактивных сессий: {Count}", cleanedCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при очистке неактивных сессий");
            }
        }

        private async Task CleanupCancelledJobs()
        {
            try
            {
                var cancelledJobs = await _jobRepository.GetJobsByStatusAsync(TranslationJobStatus.Cancelled);
                var oldCancelledJobs = cancelledJobs.Where(j =>
                    j.CompletedAt.HasValue &&
                    DateTime.UtcNow - j.CompletedAt.Value > TimeSpan.FromDays(1));

                var cleanedCount = 0;
                foreach (var job in oldCancelledJobs)
                {
                    await _jobRepository.DeleteAsync(job.Id);
                    cleanedCount++;
                }

                _logger.LogInformation("Удалено отмененных задач: {Count}", cleanedCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при очистке отмененных задач");
            }
        }
    }
}