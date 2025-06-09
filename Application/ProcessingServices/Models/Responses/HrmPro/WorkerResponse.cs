using System.Text.Json.Serialization;

namespace Contacts.Application.ProcessingServices.Models.Responses.HrmPro;

public record WorkerResponse
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
}