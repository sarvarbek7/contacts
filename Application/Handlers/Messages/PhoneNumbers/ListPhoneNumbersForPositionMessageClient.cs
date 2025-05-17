namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record ListPhoneNumbersForPositionMessageClient(int OrganizationId,
                                                 int PositionId);