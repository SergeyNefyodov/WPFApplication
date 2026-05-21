using System.Text.Json.Serialization;

namespace WPFApplication.DataExchangeExample
{
    public class WindowDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("height")]
        public double Height { get; set; }
    }
}
