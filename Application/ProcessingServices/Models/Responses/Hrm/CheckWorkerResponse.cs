using System.Text.Json.Serialization;

namespace Contacts.Application.ProcessingServices.Models.Responses.Hrm;

public record CheckWorkerResponse
{
    [JsonPropertyName("message")]
    public string Message {get; init;}

    [JsonPropertyName("worker")]
    public Worker Worker { get; init;}
}