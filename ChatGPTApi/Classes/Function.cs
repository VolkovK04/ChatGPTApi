using System.Text.Json.Serialization;

namespace ChatGPTApi
{
    internal class Function
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("parameters")]
        public object Parameters { get; set; }
    }
}
