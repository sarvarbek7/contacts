using Contacts.Domain.Accounts;
using Contacts.Domain.Users;
using Domain.Entities;

namespace Contacts.Domain.PhoneNumbers;

public class PositionAssignment : IEntity<Guid>,
    ICreatedByAccountAudit
{
    public Guid Id { get; init; }
    public int PositionId { get; init; }
    public int OrganizationId { get; init; }
    public int DepartmentId { get; init; }
    public int InnerPositionId { get; init; }
    public required PhoneNumber PhoneNumber { get; set; }
    public Guid PhoneNumberId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Account? CreatedBy { get; set; }
    public int? CreatedById { get; set; }
    public List<User> Users { get; set; } = [];

    public void AssignUser(User user,
                           int accountId)
    {
        if (Users.Any(u => u.Id == user.Id))
        {
            return;
        }

        Users.Add(user);
    }

    public void UnAssignUser(int userId, int accountId)
    {
        var user = Users.FirstOrDefault(u => u.Id == userId);
        if (user != null)
        {
            Users.Remove(user);
        }
    }
}