using Translator.Domain.Interfaces.Repositories;
using Translator.Domain.Interfaces.Services;
using Translator.Domain.Entities;
using Translator.CommonLib.Models;

namespace Translator.Application.Services
{
    public class TranslationOrchestrator : ITranslationOrchestrator
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAiTranslationService _aiService;
        private readonly ITranslationJobProcessor _jobProcessor;

        public TranslationOrchestrator(
            ISessionRepository sessionRepository,
            IJobRepository jobRepository,
            IUserRepository userRepository,
            IAiTranslationService aiService,
            ITranslationJobProcessor jobProcessor)
        {
            _sessionRepository = sessionRepository;
            _jobRepository = jobRepository;
            _userRepository = userRepository;
            _aiService = aiService;
            _jobProcessor = jobProcessor;
        }

        public async Task<TranslationSession> CreateSessionAsync(string connectionId, string? userId = null)
        {
            var session = new TranslationSession(connectionId);

            if (userId != null)
            {
                var user = await _userRepository.GetByIdAsync(userId);
                user?.AddSessionToHistory(session.Id);

                if (user != null)
                {
                    await _userRepository.UpdateAsync(user);
                }
            }

            await _sessionRepository.AddAsync(session);
            return session;
        }

        public async Task<TranslationJob> ProcessTranslationAsync(string sessionId, TranslationRequest request)
        {
            var session = await _sessionRepository.GetByIdAsync(sessionId);
            if (session == null)
                throw new InvalidOperationException($"Сессия {sessionId} не найдена");

            session.AddRequest(request);

            var job = new TranslationJob(sessionId, request);
            await _jobRepository.AddAsync(job);

            await _sessionRepository.UpdateAsync(session);

            await _jobProcessor.EnqueueAsync(job.Id);

            return job;
        }

        public async Task<TranslationResponse> GetJobResultAsync(string jobId)
        {
            var job = await _jobRepository.GetByIdAsync(jobId);
            if (job == null)
                throw new InvalidOperationException($"Задача {jobId} не найдена");

            if (job.Status == TranslationJobStatus.Completed && job.Response != null)
                return job.Response;

            if (job.Status == TranslationJobStatus.Failed)
                throw new InvalidOperationException($"Задача завершилась с ошибкой: {job.ErrorMessage}");

            throw new InvalidOperationException($"Задача еще не завершена. Текущий статус: {job.Status}");
        }

        public async Task CloseSessionAsync(string sessionId)
        {
            var session = await _sessionRepository.GetByIdAsync(sessionId);
            if (session != null)
            {
                session.Close();
                await _sessionRepository.UpdateAsync(session);
            }
        }

        public async Task<TranslationSession?> GetSessionByConnectionIdAsync(string connectionId)
        {
            return await _sessionRepository.GetByConnectionIdAsync(connectionId);
        }

        public async Task<List<TranslationSession>> GetActiveSessionsAsync()
        {
            return await _sessionRepository.GetActiveSessionsAsync();
        }

        public async Task CleanupExpiredSessionsAsync(TimeSpan timeout)
        {
            var expiredSessions = await _sessionRepository.GetExpiredSessionsAsync(timeout);

            foreach (var session in expiredSessions)
            {
                session.Close();
                await _sessionRepository.UpdateAsync(session);
            }
        }
    }
}