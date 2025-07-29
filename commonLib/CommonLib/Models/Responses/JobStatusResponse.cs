using Newtonsoft.Json;
using System;
using Translator.CommonLib.Models;

namespace CommonLib.Models.Responses
{
    public class JobStatusResponse
    {
        [JsonProperty("jobId")]
        public string JobId { get; set; } = string.Empty;

        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;

        [JsonProperty("result")]
        public TranslationResponse? Result { get; set; }

        [JsonProperty("error")]
        public string? Error { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}