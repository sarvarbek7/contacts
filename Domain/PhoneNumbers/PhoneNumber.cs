using Contacts.Domain.Accounts;
using Contacts.Domain.Users;
using Domain.Entities;

namespace Contacts.Domain.PhoneNumbers;

public class PhoneNumber : IEntity<Guid>,
    ICreatedByAccountAudit,
    IUpdatedByAccountAudit,
    IDeletedByAccountAudit
{
    public Guid Id { get; init; }
    public required string Number { get; set; }
    public int? ActiveAssignedUserId { get; set; }
    public User? ActiveAssignedUser { get; set; }
    public List<UserPhoneNumber> UsersHistory { get; set; } = [];
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

    public void AssignUser(User user, int accountId)
    {
        ActiveAssignedUser = user;

        foreach (var history in UsersHistory.Where(x => x.IsActive))
        {
            history.IsActive = false;
        }

        var newHistory = new UserPhoneNumber()
        {
            // TODO: Fix later accountId
            CreatedById = null, // accountId,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            User = user
        };

        UsersHistory.Add(newHistory);
    }
}