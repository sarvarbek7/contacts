using System.Text.Json.Serialization;

namespace Contacts.Application.ProcessingServices.Models;

public record OrganizationStructure
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("label")]
    public required string Label { get; init; }

    [JsonPropertyName("parent_id")]
    public int? ParentId { get; init; }

    [JsonPropertyName("is_organization")]
    public bool IsOrganization { get; init; }

    [JsonPropertyName("children")]
    public List<OrganizationStructure> Children { get; init; } = [];
}