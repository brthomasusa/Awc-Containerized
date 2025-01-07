using System.Text.Json.Serialization;

namespace WebUI.Exceptions
{
    public class ProblemDetailResponse
    {
        [JsonPropertyName("detail")]
        public string? Detail { get; set; }

        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }
    }
}