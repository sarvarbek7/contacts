using System.Text.Json.Serialization;

namespace Contacts.Contracts.Users;

public record HrmUser
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("photo")]
    public required string Photo { get; init; }

    [JsonPropertyName("last_name")]
    public required string LastName { get; init; }

    [JsonPropertyName("first_name")]
    public required string FirstName { get; init; }

    [JsonPropertyName("middle_name")]
    public string? MiddleName { get; init; }
}