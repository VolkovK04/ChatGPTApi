using System.Text.Json.Serialization;

namespace ChatGPTApi
{
    internal class Request
    {
        [JsonPropertyName("model")]
        public string ModelId { get; set; } = "";
        [JsonPropertyName("messages")]
        public List<Message> Messages { get; set; } = new();
        //[JsonPropertyName("functions")]
        //public List<Function> Functions { get; set; } = new();
        [JsonPropertyName("stream")]

        public bool Stream { get; set; } = false;
    }
}
