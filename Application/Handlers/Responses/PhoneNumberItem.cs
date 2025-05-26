namespace Contacts.Application.Handlers.Responses;

public record PhoneNumberItem
{
    public Guid Id { get; init; }
    public required string Number { get; init; }
    public required string Type { get; set; }
}