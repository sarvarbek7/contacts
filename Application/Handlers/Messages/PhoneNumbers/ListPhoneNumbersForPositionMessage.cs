namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record ListPhoneNumbersForPositionMessage(int OrganizationId,
                                                 int? PositionId);