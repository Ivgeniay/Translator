using Translator.CommonLib.Models;

namespace Translator.Domain.Entities
{
    public class TranslationSession
    {
        public string Id { get; private set; }
        public string ConnectionId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime LastActivityAt { get; private set; }
        public TranslationSessionStatus Status { get; private set; }
        public List<TranslationRequest> Requests { get; private set; } = new List<TranslationRequest>();
        public List<TranslationResponse> Results { get; private set; } = new List<TranslationResponse>();

        private TranslationSession() { }

        public TranslationSession(string connectionId)
        {
            Id = Guid.NewGuid().ToString();
            ConnectionId = connectionId;
            CreatedAt = DateTime.UtcNow;
            LastActivityAt = DateTime.UtcNow;
            Status = TranslationSessionStatus.Active;
        }

        public void UpdateActivity()
        {
            LastActivityAt = DateTime.UtcNow;
        }

        public void AddRequest(TranslationRequest request)
        {
            Requests.Add(request);
            UpdateActivity();
        }

        public void AddResult(TranslationResponse result)
        {
            Results.Add(result);
            UpdateActivity();
        }

        public void Close()
        {
            Status = TranslationSessionStatus.Closed;
            UpdateActivity();
        }

        public bool IsExpired(TimeSpan timeout)
        {
            return DateTime.UtcNow - LastActivityAt > timeout;
        }
    }

    public enum TranslationSessionStatus
    {
        Active = 0,
        Processing = 1,
        Closed = 2,
        Error = 3
    }
}