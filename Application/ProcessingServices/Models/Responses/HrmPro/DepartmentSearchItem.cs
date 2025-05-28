using System.Text.Json.Serialization;

namespace Contacts.Application.ProcessingServices.Models.Responses.HrmPro;

public record DepartmentSearchItem
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("organization")]
    public required OrganizationItem Organization { get; init; }
}