namespace Contacts.Application.ProcessingServices.Models;

public record PositionChangedMessage
{
    public int PositionId { get; set; }
    public int UserExternalId { get; set; }
}