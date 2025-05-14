using System.Text.Json.Serialization;

namespace Contacts.Application.ProcessingServices.Models.Responses.HrmPro;

public record DepartmentPosition
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("position")]
    public required PositionItem Position { get; init; }
}