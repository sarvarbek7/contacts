using System.Text.Json.Serialization;

namespace Contacts.Application.ProcessingServices.Models.Responses.Hrm;

public record CheckWorkerResponse
{
    [JsonPropertyName("message")]
    public required string Message {get; init;}

    [JsonPropertyName("worker")]
    public required Worker Worker { get; init;}
}