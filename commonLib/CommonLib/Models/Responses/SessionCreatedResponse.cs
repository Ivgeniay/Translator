using Newtonsoft.Json;
using System;

namespace CommonLib.Models.Responses
{
    public class SessionCreatedResponse
    {
        [JsonProperty("sessionId")]
        public string SessionId { get; set; } = string.Empty;

        [JsonProperty("connectionId")]
        public string ConnectionId { get; set; } = string.Empty;

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}