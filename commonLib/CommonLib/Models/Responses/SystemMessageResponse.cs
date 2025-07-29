using Newtonsoft.Json;
using System;

namespace CommonLib.Models.Responses
{
    public class SystemMessageResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("type")]
        public string Type { get; set; } = "System";

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}