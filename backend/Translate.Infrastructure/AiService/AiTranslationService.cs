using Translate.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using CommonLib.Models;

namespace Translate.Infrastructure.AiService
{
    public class AiTranslationService : IAiTranslationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public AiTranslationService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient(nameof(IAiTranslationService));
            _baseUrl = configuration["AiService:BaseUrl"] ?? "http://localhost:3000";
        }

        public Task<Language> DetectLanguageAsync(byte[] audioData)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsHealthyAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> SpeechToTextAsync(byte[] audioData, AudioSettings audioSettings, Language? sourceLanguage = null)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> TextToSpeechAsync(string text, VoiceSettings voiceSettings, AudioSettings audioSettings)
        {
            throw new NotImplementedException();
        }

        public Task<TranslationResponse> TranslateAsync(TranslationRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<string> TranslateTextAsync(string text, Language sourceLanguage, Language targetLanguage)
        {
            throw new NotImplementedException();
        }
    }
}