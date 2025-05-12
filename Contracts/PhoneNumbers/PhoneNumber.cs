using Contacts.Contracts.Users;

namespace Contacts.Contracts.PhoneNumbers;

public record PhoneNumber
{
    public Guid Id { get; init; }
    public required string Number { get; set; }
    public UserListItem? ActiveAssignedUser { get; set; }
    public List<UserHistoryItem> History { get; set; } = [];
    public List<PositionHistoryItem> PositionHistory { get; set; } = [];
}