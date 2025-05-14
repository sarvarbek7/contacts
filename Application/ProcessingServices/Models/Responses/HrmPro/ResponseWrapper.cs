using System.Text.Json.Serialization;

namespace Contacts.Application.ProcessingServices.Models.Responses.HrmPro;

public record ResponseWrapper<T>
{
    [JsonPropertyName("message")]
    public bool Message { get; init; }
    
    [JsonPropertyName("error")]
    public bool Error { get; init; }

    [JsonPropertyName("data")]
    public required T Data { get; init; }
}