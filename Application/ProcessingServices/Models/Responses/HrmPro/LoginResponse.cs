using System.Text.Json.Serialization;

namespace Contacts.Application.ProcessingServices.Models.Responses.HrmPro;

public record LoginResponse
{
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; init; }

    public string TokenValue => $"Bearer {AccessToken}";
}