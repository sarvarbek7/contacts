using System.Text.Json.Serialization;
using Contacts.Domain.Users;

namespace Contacts.Application.ProcessingServices.Models;

public record Worker
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("first_name")]
    public string FirstName { get; init; }

    [JsonPropertyName("last_name")]
    public string LastName { get; init; }

    [JsonPropertyName("middle_name")]
    public string? MiddleName { get; init; }

    [JsonPropertyName("photo")]
    public string? Photo { get; init; }

    public User MapToUser(string pinfl)
    {
        return new User()
        {
            ExternalId = Id,
            FirstName = FirstName,
            LastName = LastName,
            MiddleName = MiddleName,
            PhotoUrl = Photo,
            Pinfl = pinfl,
        };
    }
}