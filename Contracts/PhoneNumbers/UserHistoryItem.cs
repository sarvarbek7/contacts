using Contacts.Contracts.Users;

namespace Contacts.Contracts.PhoneNumbers;

public record UserHistoryItem
{
    public long Id { get; init; }
    public bool IsActive { get; set; }
    public UserListItem? User { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? RemovedAt { get; set; }
}