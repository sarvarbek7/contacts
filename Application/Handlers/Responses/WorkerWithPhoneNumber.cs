using System.Text.Json.Serialization;
using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;

namespace Contacts.Application.Handlers.Responses;

public record WorkerWithPhoneNumber
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("organization")]
    public required OrganizationItem Organization { get; init; }

    [JsonPropertyName("department")]
    public required DepartmentItem Department { get; init; }

    [JsonPropertyName("department_position")]
    public required DepartmentPosition DepartmentPosition { get; init; }

    [JsonPropertyName("worker")]
    public required Worker Worker { get; init; }

    public List<PhoneNumberItem> PhoneNumbers {get; init;} = [];
}