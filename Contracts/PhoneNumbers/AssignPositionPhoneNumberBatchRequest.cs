namespace Contacts.Contracts.PhoneNumbers;

public record AssignPositionPhoneNumberBatchRequest(int PositionId,
                                               string Organization,
                                               string Department,
                                               string Position,
                                               int OrganizationId,
                                               IEnumerable<Guid> PhoneIds) : AssignPositionPhoneNumberRequest(PositionId,
                                                                                                 Organization,
                                                                                                 Department,
                                                                                                 Position,
                                                                                                 OrganizationId);