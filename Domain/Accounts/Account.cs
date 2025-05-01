using Domain.Entities;

namespace Contacts.Domain.Accounts;

public class Account : IEntity<int>,
    ICreatedByAccountAudit,
    IUpdatedByAccountAudit,
    IDeletedByAccountAudit
{
    public int Id { get; init; }
    public string Login { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; }
    public Account? CreatedBy { get; set; }
    public int? CreatedById { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Account? UpdatedBy { get; set; }
    public int? UpdatedById { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Account? DeletedBy { get; set; }
    public int? DeletedById { get; set; }
    public bool IsDeleted { get; set; }
}
