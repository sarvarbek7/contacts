using Domain.Entities;

namespace Contacts.Domain.Accounts;

public interface IDeletedByAccountAudit : IDeletionAuditableEntity<Account, int>;