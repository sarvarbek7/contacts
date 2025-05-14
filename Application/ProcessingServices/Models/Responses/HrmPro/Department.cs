using System.Text.Json.Serialization;

namespace Contacts.Application.ProcessingServices.Models.Responses.HrmPro;

public record Department
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("level")]
    public Level? Level { get; init; }

    [JsonPropertyName("parent")]
    public Department? Parent { get; init; }

    [JsonPropertyName("children")]
    public ICollection<Department> Children { get; init; } = [];
}