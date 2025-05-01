namespace Contacts.Contracts.PhoneNumbers;

public record CreatePhoneNumberRequest(string Number, int? ExternalId);

public record UpdatePhoneNumber(string Number);