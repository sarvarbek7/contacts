namespace Contacts.Application.Handlers.Messages;

public interface IAuditableMessage
{
    int UserAccountIdWhoDoesAction { get; init; }
}