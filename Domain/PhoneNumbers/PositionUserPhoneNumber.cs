using Contacts.Domain.Accounts;
using Domain.Entities;

namespace Contacts.Domain.PhoneNumbers;

public class PositionUserPhoneNumber : IEntity<long>,
    ICreatedByAccountAudit
{
    public long Id { get; init; }
    public Guid PhoneNumberId { get; init; }
    public PhoneNumber? PhoneNumber { get; set; }
    public int UserId { get; init; }
    public DateTime CreatedAt { get; set; }
    public Account? CreatedBy { get; set; }
    public int? CreatedById { get; set; }
    public Account? RemovedBy { get; set; }
    public int? RemovedById { get; set; }
    public DateTime? RemovedAt { get; set; }
}
