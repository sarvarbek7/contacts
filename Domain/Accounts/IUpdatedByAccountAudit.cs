using Domain.Entities;

namespace Contacts.Domain.Accounts;

public interface IUpdatedByAccountAudit : IUpdateAuditableEntity<Account, int>;