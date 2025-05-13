using Contacts.Contracts.Users;

namespace Contacts.Contracts.PhoneNumbers;

public record PositionHistoryItem
{
    public long Id { get; init; }
    public bool IsActive { get; init; }
    public int PositionId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? RemovedAt { get; init; }
}