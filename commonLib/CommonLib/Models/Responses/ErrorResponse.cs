using Newtonsoft.Json;
using System;

namespace CommonLib.Models.Responses
{
    public class ErrorResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("details")]
        public string? Details { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}