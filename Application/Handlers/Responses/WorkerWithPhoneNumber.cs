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

    [JsonPropertyName("post_name")]
    public string PostName { get; init; } = string.Empty;

    public List<PhoneNumberItem> PhoneNumbers { get; init; } = [];

    public static WorkerWithPhoneNumber MapFrom(WorkerResponse workerResponse, List<PhoneNumberItem> userPhoneNumbers)
    {
        return new()
            {
                Id = workerResponse.Id,
                Organization = workerResponse.Organization,
                Department = workerResponse.Department,
                DepartmentPosition = workerResponse.DepartmentPosition,
                Worker = workerResponse.Worker,
                PhoneNumbers = userPhoneNumbers,
                PostName = workerResponse.PostName,
            };
    }
}