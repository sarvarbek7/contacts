namespace Contacts.Application.Handlers.Messages.Handbooks;

public record DeleteHandbookItemMessage(int Id, Guid PhoneNumberId);