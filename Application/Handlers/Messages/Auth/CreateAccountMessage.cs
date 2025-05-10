using Contacts.Domain.Accounts;

namespace Contacts.Application.Handlers.Messages.Auth;

public record CreateAccountMessage(string Login, string Password, Role? Role, int UserAccountIdWhoDoesAction) : IAuditableMessage;