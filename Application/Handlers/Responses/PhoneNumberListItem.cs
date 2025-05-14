using System.Linq.Expressions;
using Contacts.Domain.Users;

namespace Contacts.Application.Handlers.Responses;

public record PhoneNumberListItem(Guid Id, string Number, User? AssignedUser, int? PositionId)
{
 public static Expression<Func<Domain.PhoneNumbers.PhoneNumber, PhoneNumberListItem>> To =>
        x => new PhoneNumberListItem(x.Id,
                                     x.Number,
                                     x.ActiveAssignedUser,
                                     x.ActiveAssignedPositionId);
}
