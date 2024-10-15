using System.Text.Json.Serialization;

namespace AWC.Shared.Kernel.Exceptions
{
    public sealed class ApiErrorResponse
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("errors")]
        public string[]? Errors { get; set; }        
    }
}