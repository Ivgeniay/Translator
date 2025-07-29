using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Translator.CommonLib.Models
{
    public class LanguageSettings
    {
        [Required]
        [JsonProperty("sourceLanguage")]
        public Language SourceLanguage { get; set; } = Language.Auto;

        [Required]
        [JsonProperty("targetLanguage")]
        public Language TargetLanguage { get; set; } = Language.Russian;

        [JsonProperty("autoDetectSource")]
        public bool AutoDetectSource { get; set; } = true;
    }
}