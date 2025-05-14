using System.Text.Json.Serialization;
using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;

namespace Contacts.Application.Handlers.Responses;

public record PositionWithPhoneNumber
{
    public int Id { get; init; }

    public required OrganizationItem Organization { get; init; }

    public required DepartmentItem Department { get; init; }

    [JsonPropertyName("position")]
    public required PositionItem PositionItem { get; init; }

    public List<PhoneNumberItem> PhoneNumbers {get; init;} = [];
}