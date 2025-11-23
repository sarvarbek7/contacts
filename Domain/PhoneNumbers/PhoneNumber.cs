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

    /// <summary>
    /// Biriktirilgan lavozimlar
    /// </summary>
    public List<PositionAssignment> AssignedPositions { get; set; } = [];

    /// <summary>
    /// To'g'ridan to'g'ri biriktirilgan xodim(lavozimidan qat'i nazar)
    /// </summary>
    public List<UserAssignment> AssignedUsers { get; set; } = [];

    public void AssignUser(User user, int accountId)
    {
        var existingAssignment = AssignedUsers
            .FirstOrDefault(x => x.UserId == user.Id);

        if (existingAssignment == null)
        {
            var userAssignment = new UserAssignment()
            {
                UserId = user.Id,
                User = user,
                PhoneNumber = this,
                PhoneNumberId = Id,
                CreatedById = accountId,
                CreatedAt = DateTime.UtcNow,
            };

            AssignedUsers.Add(userAssignment);
        }


    }

    public void UnAssignUser(Guid userAssignmentId, int accountId)
    {
        var userAssignment = AssignedUsers
            .FirstOrDefault(x => x.Id == userAssignmentId);

        if (userAssignment != null)
        {
            AssignedUsers.Remove(userAssignment);
        }
    }

    public void AssignPosition(int positionId,
                               int organizationId,
                               int departmentId,
                               int innerPositionId,
                               int accountId)
    {
        var positionAssignment = new PositionAssignment()
        {
            PositionId = positionId,
            OrganizationId = organizationId,
            DepartmentId = departmentId,
            InnerPositionId = innerPositionId,
            PhoneNumber = this,
            PhoneNumberId = Id,
            CreatedById = accountId,
            CreatedAt = DateTime.UtcNow,
        };

        AssignedPositions.Add(positionAssignment);
    }

    public void UnAssignPosition(Guid assignmentId, int accountId)
    {
        var assignment = AssignedPositions.FirstOrDefault(x => x.Id == assignmentId);
        if (assignment != null)
        {
            AssignedPositions.Remove(assignment);
        }
    }
}