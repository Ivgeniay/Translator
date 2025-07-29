using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System;

namespace Translator.CommonLib.Models
{
    public class TranslationResponse
    {
        [Required]
        [JsonProperty("requestId")]
        public string RequestId { get; set; } = string.Empty;

        [Required]
        [JsonProperty("sessionId")]
        public string SessionId { get; set; } = string.Empty;

        [JsonProperty("translatedAudio")]
        public byte[]? TranslatedAudio { get; set; }

        [JsonProperty("originalText")]
        public string? OriginalText { get; set; }

        [JsonProperty("translatedText")]
        public string? TranslatedText { get; set; }

        [JsonProperty("detectedLanguage")]
        public Language? DetectedLanguage { get; set; }

        [JsonProperty("processingTimeMs")]
        public long ProcessingTimeMs { get; set; }

        [JsonProperty("audioSettings")]
        public AudioSettings? AudioSettings { get; set; }

        [JsonProperty("voiceSettings")]
        public VoiceSettings? VoiceSettings { get; set; }

        [JsonProperty("status")]
        public TranslationStatus Status { get; set; } = TranslationStatus.Success;

        [JsonProperty("errorMessage")]
        public string? ErrorMessage { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public enum TranslationStatus
    {
        Success = 0,
        Processing = 1,
        Failed = 2,
        PartialSuccess = 3
    }
}