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
    public string TypeString => Type.ToString().ToLower();
    public required PhoneNumberType Type { get; set; }
    public int? ActiveAssignedOrganizationId { get; set; }
    public int? ActiveAssignedUserId { get; set; }
    public User? ActiveAssignedUser { get; set; }
    public int? ActiveAssignedPositionId { get; set; }
    public int? ActiveAssignedPositionUserId { get; set; }
    public User? ActiveAssignedPositionUser { get; set; }
    public List<UserPhoneNumber> UsersHistory { get; set; } = [];
    public List<PositionUserPhoneNumber> PositionUsersHistory { get; set; } = [];
    public List<PositionPhoneNumber> PositionHistory { get; set; } = [];
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

    public void AssignUser(User user, int organizationId, int accountId)
    {
        ActiveAssignedUser = user;
        ActiveAssignedOrganizationId = organizationId;

        foreach (var history in UsersHistory.Where(x => x.IsActive))
        {
            history.IsActive = false;
        }

        var newHistory = new UserPhoneNumber()
        {
            CreatedById = accountId,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            User = user
        };

        UsersHistory.Add(newHistory);
    }

    public void UnAssignUser(int accountId)
    {
        ActiveAssignedUserId = null;
        ActiveAssignedOrganizationId = null;

        foreach (var history in UsersHistory.Where(x => x.IsActive))
        {
            history.IsActive = false;
            history.RemovedAt = DateTime.UtcNow;
            history.RemovedById = accountId;
        }
    }

    public void AssignPositionUser(User user, int accountId)
    {
        ActiveAssignedPositionUser = user;

        var history = new PositionUserPhoneNumber()
        {
            UserId = user.Id,
            CreatedById = accountId,
            CreatedAt = DateTime.UtcNow,
        };

        PositionUsersHistory.Add(history);
    }

    public void UnAssignPositionUser(int accountId)
    {
        ActiveAssignedPositionUserId = null;

        foreach (var history in UsersHistory.Where(x => x.IsActive))
        {
            history.IsActive = false;
            history.RemovedAt = DateTime.UtcNow;
            history.RemovedById = accountId;
        }
    }

    public void AssignPosition(int positionId,
                               int organizationId,
                               string organization,
                               string department,
                               string position,
                               int accountId)
    {
        ActiveAssignedPositionId = positionId;
        ActiveAssignedOrganizationId = organizationId;

        var positionPhoneHistory = new PositionPhoneNumber()
        {
            PositionId = positionId,
            IsActive = true,
            CreatedById = accountId,
            CreatedAt = DateTime.UtcNow,
            Organization = organization,
            Department = department,
            Position = position,
        };

        PositionHistory.Add(positionPhoneHistory);
    }

    public void UnAssignPosition(int accountId)
    {
        ActiveAssignedPositionId = null;
        ActiveAssignedPositionUserId = null;
        ActiveAssignedOrganizationId = null;

        foreach (var history in PositionHistory.Where(x => x.IsActive))
        {
            history.IsActive = false;
            history.RemovedAt = DateTime.UtcNow;
            history.RemovedById = accountId;
        }
    }
}