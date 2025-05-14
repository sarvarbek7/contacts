using System.Text.Json.Serialization;

namespace Contacts.Application.ProcessingServices.Models.Responses.HrmPro;

public record Position
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("organization")]
    public required OrganizationItem Organization { get; init; }

    [JsonPropertyName("department")]
    public required DepartmentItem Department { get; init; }

    [JsonPropertyName("position")]
    public required PositionItem PositionItem { get; init; }
}