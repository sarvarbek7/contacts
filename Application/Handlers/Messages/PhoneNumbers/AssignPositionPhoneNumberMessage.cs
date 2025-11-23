namespace Contacts.Application.Handlers.Messages.PhoneNumbers;

public record AssignPositionPhoneNumberMessage(int PositionId,
                                               Guid PhoneNumberId,
                                               int OrganizationId,
                                               int DepartmentId,
                                               int InnerPositionId,
                                               int UserAccountIdWhoDoesAction) : IAuditableMessage;