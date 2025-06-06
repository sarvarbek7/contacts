using System.Text.Json.Serialization;

namespace Contacts.Application.ProcessingServices.Models.Responses.Hrm;

public record LoginResponse
{
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; init; }

    [JsonPropertyName("token_type")]
    public required string TokenType{ get; init; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; init; }

    public string TokenValue => $"{TokenType} {AccessToken}";
}