using Domain.Entities;

namespace Contacts.Domain.Accounts;

public interface ICreatedByAccountAudit : ICreationAuditableEntity<Account, int>;