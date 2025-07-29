using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System;

namespace Translator.CommonLib.Models
{
    public class TranslationRequest
    {
        [Required]
        [JsonProperty("sessionId")]
        public string SessionId { get; set; } = string.Empty;

        [Required]
        [JsonProperty("audioData")]
        public byte[] AudioData { get; set; } = Array.Empty<byte>();

        [Required]
        [JsonProperty("audioSettings")]
        public AudioSettings AudioSettings { get; set; } = new AudioSettings();

        [Required]
        [JsonProperty("languageSettings")]
        public LanguageSettings LanguageSettings { get; set; } = new LanguageSettings();

        [Required]
        [JsonProperty("voiceSettings")]
        public VoiceSettings VoiceSettings { get; set; } = new VoiceSettings();

        [JsonProperty("includeTextResponse")]
        public bool IncludeTextResponse { get; set; } = false;

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}