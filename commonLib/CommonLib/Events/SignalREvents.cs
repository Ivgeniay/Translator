namespace Translator.CommonLib.Events
{
    public static class SignalREvents
    {
        public const string SessionCreated = nameof(SessionCreated);
        public const string TranslationStarted = nameof(TranslationStarted);
        public const string TranslationCompleted = nameof(TranslationCompleted);
        public const string TranslationFailed = nameof(TranslationFailed);
        public const string JobStatus = nameof(JobStatus);
        public const string JobStatusChanged = nameof(JobStatusChanged);
        public const string SessionClosed = nameof(SessionClosed);
        public const string SystemMessage = nameof(SystemMessage);
        public const string Error = nameof(Error);
    }
}