using ChatGPTApi.Classes;
using System.Text.Json.Serialization;

namespace ChatGPTApi.Classes
{
    internal class Choice
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }
        [JsonPropertyName("message")]
        public Message Message { get; set; } = new();
        [JsonPropertyName("delta")]
        public DeltaMessage DeltaMessage { get; set; } = new();

        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; } = "";
    }
}
