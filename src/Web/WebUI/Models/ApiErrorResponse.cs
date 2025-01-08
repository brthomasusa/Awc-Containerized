using System.Text.Json.Serialization;

namespace WebUI.Models;

public class ApiErrorResponse(string errMessage)
{
    [JsonPropertyName("message")]
    public string? Message { get; set; } = errMessage;
}