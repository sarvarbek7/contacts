namespace Contacts.Contracts.PhoneNumbers;

public record AssignPositionPhoneNumberRequest(int PositionId,
                                               int OrganizationId,
                                               int DepartmentId,
                                               int InnerPositionId);