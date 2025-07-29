using Newtonsoft.Json;
using System;

namespace CommonLib.Models.Responses
{
    public class JobStatusChangedResponse
    {
        [JsonProperty("jobId")]
        public string JobId { get; set; } = string.Empty;

        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}