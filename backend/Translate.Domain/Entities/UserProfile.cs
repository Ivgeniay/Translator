namespace Translate.Domain.Entities
{
    public class UserProfile
    {
        public string Id { get; private set; }
        public string? Username { get; private set; }
        public string? Email { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime LastLoginAt { get; private set; }
        public UserSettings Settings { get; private set; }
        public List<string> SessionHistory { get; private set; } = new();
        public UserStatus Status { get; private set; }

        private UserProfile() { }

        public UserProfile(string? username = null, string? email = null)
        {
            Id = Guid.NewGuid().ToString();
            Username = username;
            Email = email;
            CreatedAt = DateTime.UtcNow;
            LastLoginAt = DateTime.UtcNow;
            Settings = new UserSettings();
            Status = UserStatus.Active;
        }

        public void UpdateLastLogin()
        {
            LastLoginAt = DateTime.UtcNow;
        }

        public void UpdateSettings(UserSettings newSettings)
        {
            Settings = newSettings;
        }

        public void AddSessionToHistory(string sessionId)
        {
            SessionHistory.Add(sessionId);

            if (SessionHistory.Count > 100)
            {
                SessionHistory.RemoveAt(0);
            }
        }

        public void Deactivate()
        {
            Status = UserStatus.Inactive;
        }

        public void Activate()
        {
            Status = UserStatus.Active;
        }

        public bool IsActive => Status == UserStatus.Active;
    }
}