using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CommonLib.Models
{
    public class VoiceSettings
    {
        [Required]
        [JsonProperty("voiceId")]
        public string VoiceId { get; set; } = "alloy";

        [Range(0.1, 2.0)]
        [JsonProperty("speed")]
        public float Speed { get; set; } = 1.0f;

        [Range(0.1, 2.0)]
        [JsonProperty("pitch")]
        public float Pitch { get; set; } = 1.0f;

        [Range(0.0, 1.0)]
        [JsonProperty("volume")]
        public float Volume { get; set; } = 1.0f;

        [JsonProperty("accent")]
        public VoiceAccent Accent { get; set; } = VoiceAccent.Neutral;
    }
}