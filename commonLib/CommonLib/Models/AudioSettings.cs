using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System;

namespace Translator.CommonLib.Models
{
    public class AudioSettings
    {
        [Range(8000, 48000)]
        [JsonProperty("sampleRate")]
        public int SampleRate { get; set; } = 16000;

        [Range(64, 320)]
        [JsonProperty("bitRate")]
        public int BitRate { get; set; } = 128;

        [Required]
        [JsonProperty("format")]
        public AudioFormat Format { get; set; } = AudioFormat.Wav;

        [Range(1, 30)]
        [JsonProperty("segmentDurationSeconds")]
        public int SegmentDurationSeconds { get; set; } = 5;

        [JsonProperty("enableNoiseReduction")]
        public bool EnableNoiseReduction { get; set; } = true;
    }
}