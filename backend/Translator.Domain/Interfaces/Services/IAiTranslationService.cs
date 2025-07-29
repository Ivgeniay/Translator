using Translator.CommonLib.Models;

namespace Translator.Domain.Interfaces.Services
{
    public interface IAiTranslationService
    {
        Task<TranslationResponse> TranslateAsync(TranslationRequest request);
        Task<bool> IsHealthyAsync();
        Task<Language> DetectLanguageAsync(byte[] audioData);
        Task<string> SpeechToTextAsync(byte[] audioData, AudioSettings audioSettings, Language? sourceLanguage = null);
        Task<string> TranslateTextAsync(string text, Language sourceLanguage, Language targetLanguage);
        Task<byte[]> TextToSpeechAsync(string text, VoiceSettings voiceSettings, AudioSettings audioSettings);
    }
}