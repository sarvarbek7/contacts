using Contacts.Contracts.Users;

namespace Contacts.Contracts.PhoneNumbers;

public record PhoneNumberHistoryItem
{
    public long Id { get; init; }
    public bool IsActive { get; set; }
    public PhoneNumberListItem? PhoneNumber {get; set;}
    public DateTime CreatedAt { get; set; }
    public DateTime? RemovedAt { get; set; }
}