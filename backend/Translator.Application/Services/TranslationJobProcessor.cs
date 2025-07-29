using Translator.Domain.Interfaces.Repositories;
using Translator.Domain.Interfaces.Services;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Translator.Domain.Entities;

namespace Translator.Application.Services
{
    public class TranslationJobProcessor : ITranslationJobProcessor
    {
        private readonly ConcurrentQueue<string> _jobQueue;
        private readonly IJobRepository _jobRepository;
        private readonly IAiTranslationService _aiService;
        private readonly INotificationService _notificationService;
        private readonly ISessionRepository _sessionRepository;
        private readonly ILogger<TranslationJobProcessor> _logger;
        private readonly SemaphoreSlim _processingSemaphore;
        private readonly int _maxConcurrentJobs;

        public TranslationJobProcessor(
            IJobRepository jobRepository,
            IAiTranslationService aiService,
            INotificationService notificationService,
            ISessionRepository sessionRepository,
            ILogger<TranslationJobProcessor> logger)
        {
            _jobQueue = new ConcurrentQueue<string>();
            _jobRepository = jobRepository;
            _aiService = aiService;
            _notificationService = notificationService;
            _sessionRepository = sessionRepository;
            _logger = logger;
            _maxConcurrentJobs = 5;
            _processingSemaphore = new SemaphoreSlim(_maxConcurrentJobs, _maxConcurrentJobs);
        }

        public async Task EnqueueAsync(string jobId)
        {
            _jobQueue.Enqueue(jobId);
            _logger.LogInformation("Задача {JobId} добавлена в очередь", jobId);

            _ = Task.Run(async () => await ProcessNextJobAsync());
        }

        public async Task<TranslationJob?> DequeueAsync()
        {
            if (_jobQueue.TryDequeue(out var jobId))
            {
                return await _jobRepository.GetByIdAsync(jobId);
            }
            return null;
        }

        public async Task ProcessJobAsync(TranslationJob job)
        {
            await _processingSemaphore.WaitAsync();

            try
            {
                _logger.LogInformation("Начинаем обработку задачи {JobId}", job.Id);

                job.Start();
                await _jobRepository.UpdateAsync(job);

                var session = await _sessionRepository.GetByIdAsync(job.SessionId);
                if (session == null)
                {
                    job.Fail("Сессия не найдена");
                    await _jobRepository.UpdateAsync(job);
                    return;
                }

                await _notificationService.NotifyJobStatusChangedAsync(
                    session.ConnectionId, job.Id, TranslationJobStatus.Processing);

                var response = await _aiService.TranslateAsync(job.Request);

                job.Complete(response);
                session.AddResult(response);

                await _jobRepository.UpdateAsync(job);
                await _sessionRepository.UpdateAsync(session);

                await _notificationService.NotifyTranslationCompletedAsync(
                    session.ConnectionId, response);

                _logger.LogInformation("Задача {JobId} успешно завершена", job.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке задачи {JobId}", job.Id);

                job.Fail(ex.Message);
                await _jobRepository.UpdateAsync(job);

                var session = await _sessionRepository.GetByIdAsync(job.SessionId);
                if (session != null)
                {
                    await _notificationService.NotifyTranslationFailedAsync(
                        session.ConnectionId, job.Id, ex.Message);
                }

                if (job.CanRetry(3))
                {
                    job.IncrementRetry();
                    await _jobRepository.UpdateAsync(job);
                    await EnqueueAsync(job.Id);
                    _logger.LogInformation("Задача {JobId} поставлена на повторную обработку", job.Id);
                }
            }
            finally
            {
                _processingSemaphore.Release();
            }
        }

        public async Task RetryFailedJobsAsync()
        {
            var failedJobs = await _jobRepository.GetFailedJobsAsync();

            foreach (var job in failedJobs.Where(j => j.CanRetry(3)))
            {
                job.IncrementRetry();
                await _jobRepository.UpdateAsync(job);
                await EnqueueAsync(job.Id);

                _logger.LogInformation("Повторная обработка задачи {JobId}", job.Id);
            }
        }

        public async Task<int> GetQueueSizeAsync()
        {
            return await Task.FromResult(_jobQueue.Count);
        }

        public async Task<List<string>> GetPendingJobIdsAsync()
        {
            return await Task.FromResult(_jobQueue.ToList());
        }

        public async Task CancelJobAsync(string jobId)
        {
            var job = await _jobRepository.GetByIdAsync(jobId);
            if (job != null && job.Status == TranslationJobStatus.Pending)
            {
                job.Fail("Отменено пользователем");
                await _jobRepository.UpdateAsync(job);

                _logger.LogInformation("Задача {JobId} отменена", jobId);
            }
        }

        public async Task ClearQueueAsync()
        {
            while (_jobQueue.TryDequeue(out _)) { }
            _logger.LogInformation("Очередь задач очищена");
            await Task.CompletedTask;
        }

        private async Task ProcessNextJobAsync()
        {
            var job = await DequeueAsync();
            if (job != null)
            {
                await ProcessJobAsync(job);
            }
        }
    }
}