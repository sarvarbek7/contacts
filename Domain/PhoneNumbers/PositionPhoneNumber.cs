using Contacts.Domain.Accounts;
using Domain.Entities;

namespace Contacts.Domain.PhoneNumbers;

public class PositionPhoneNumber : IEntity<long>,
    ICreatedByAccountAudit
{
    public long Id { get; init; }
    public bool IsActive { get; set; }
    public Guid PhoneNumberId { get; init; }
    public PhoneNumber? PhoneNumber { get; set; }
    public required string Organization { get; set; }
    public required string Department { get; set; }
    public required string Position { get; set; }
    public int PositionId { get; init; }
    public DateTime CreatedAt { get; set; }
    public Account? CreatedBy { get; set; }
    public int? CreatedById { get; set; }
    public Account? RemovedBy { get; set; }
    public int? RemovedById { get; set; }
    public DateTime? RemovedAt { get; set; }
}
