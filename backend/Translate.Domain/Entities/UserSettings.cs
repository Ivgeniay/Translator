using CommonLib.Models;

namespace Translate.Domain.Entities
{
    public class UserSettings
    {
        public AudioSettings DefaultAudioSettings { get; set; } = new();
        public LanguageSettings DefaultLanguageSettings { get; set; } = new();
        public VoiceSettings DefaultVoiceSettings { get; set; } = new();
        public bool SaveTranslationHistory { get; set; } = true;
        public bool AutoDetectLanguage { get; set; } = true;
        public bool IncludeTextInResponse { get; set; } = false;
    }
}