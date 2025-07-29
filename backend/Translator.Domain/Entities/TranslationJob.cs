using Translator.CommonLib.Models;

namespace Translator.Domain.Entities
{
    public class TranslationJob
    {
        public string Id { get; private set; }
        public string SessionId { get; private set; }
        public TranslationRequest Request { get; private set; }
        public TranslationJobStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? StartedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public TranslationResponse? Response { get; private set; }
        public string? ErrorMessage { get; private set; }
        public int RetryCount { get; private set; }

        private TranslationJob() { }

        public TranslationJob(string sessionId, TranslationRequest request)
        {
            Id = Guid.NewGuid().ToString();
            SessionId = sessionId;
            Request = request;
            Status = TranslationJobStatus.Pending;
            CreatedAt = DateTime.UtcNow;
            RetryCount = 0;
        }

        public void Start()
        {
            if (Status != TranslationJobStatus.Pending && Status != TranslationJobStatus.Failed)
                throw new InvalidOperationException($"Невозможно запустить job в статусе {Status}");

            Status = TranslationJobStatus.Processing;
            StartedAt = DateTime.UtcNow;
        }

        public void Complete(TranslationResponse response)
        {
            if (Status != TranslationJobStatus.Processing)
                throw new InvalidOperationException($"Невозможно завершить job в статусе {Status}");

            Response = response;
            Status = TranslationJobStatus.Completed;
            CompletedAt = DateTime.UtcNow;
        }

        public void Fail(string errorMessage)
        {
            Status = TranslationJobStatus.Failed;
            ErrorMessage = errorMessage;
            CompletedAt = DateTime.UtcNow;
        }

        public void IncrementRetry()
        {
            RetryCount++;
        }

        public bool CanRetry(int maxRetries)
        {
            return Status == TranslationJobStatus.Failed && RetryCount < maxRetries;
        }

        public TimeSpan? GetProcessingTime()
        {
            if (StartedAt == null || CompletedAt == null)
                return null;

            return CompletedAt - StartedAt;
        }
    }

    public enum TranslationJobStatus
    {
        Pending = 0,
        Processing = 1,
        Completed = 2,
        Failed = 3,
        Cancelled = 4
    }
}