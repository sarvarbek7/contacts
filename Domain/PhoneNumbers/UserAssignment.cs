using Contacts.Domain.Accounts;
using Contacts.Domain.Users;
using Domain.Entities;

namespace Contacts.Domain.PhoneNumbers;

public class UserAssignment : IEntity<Guid>,
    ICreatedByAccountAudit
{
    public Guid Id { get; init; }
    public int UserId { get; init; }
    public User? User { get; set; }
    public required PhoneNumber PhoneNumber { get; set; }
    public Guid PhoneNumberId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Account? CreatedBy { get; set; }
    public int? CreatedById { get; set; }
    public List<PositionAssignment> PositionAssignments { get; set; } = [];
}