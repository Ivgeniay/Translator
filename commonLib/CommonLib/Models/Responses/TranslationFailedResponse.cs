using Newtonsoft.Json;
using System;

namespace CommonLib.Models.Responses
{
    public class TranslationFailedResponse
    {
        [JsonProperty("jobId")]
        public string? JobId { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; } = string.Empty;

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}