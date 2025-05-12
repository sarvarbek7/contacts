namespace Contacts.Contracts.PhoneNumbers;

public record AssignPositionPhoneNumberRequest(int PositionId,
                                               string Organization,
                                               string Department,
                                               string Position);