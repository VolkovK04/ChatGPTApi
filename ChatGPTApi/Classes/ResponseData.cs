using System.Text.Json.Serialization;

namespace ChatGPTApi.Classes
{
    internal class ResponseData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("object")]
        public string Object { get; set; } = string.Empty;
        [JsonPropertyName("created")]
        public ulong Created { get; set; }
        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;
        [JsonPropertyName("choices")]
        public List<Choice> Choices { get; set; } = new();
        [JsonPropertyName("usage")]
        public Usage Usage { get; set; } = new();
    }
}
