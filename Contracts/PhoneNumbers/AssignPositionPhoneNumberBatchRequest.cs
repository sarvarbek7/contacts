namespace Contacts.Contracts.PhoneNumbers;

public record AssignPositionPhoneNumberBatchRequest(int PositionId,
                                                    int OrganizationId,
                                                    int DepartmentId,
                                                    int InnerPositionId,
                                                    IEnumerable<Guid> PhoneIds) : AssignPositionPhoneNumberRequest(PositionId,
                                                                                                                   OrganizationId,
                                                                                                                   DepartmentId,
                                                                                                                   InnerPositionId);