using System.Text.Json.Serialization;

namespace Contacts.Application.ProcessingServices.Models.Responses.HrmPro;

public record Level
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }
}