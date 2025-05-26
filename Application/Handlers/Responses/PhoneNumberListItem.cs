using System.Linq.Expressions;
using Contacts.Domain.PhoneNumbers;
using Contacts.Domain.Users;

namespace Contacts.Application.Handlers.Responses;

public record PhoneNumberListItem(Guid Id, string Number, PhoneNumberType Type, User? AssignedUser, int? PositionId)
{
 public static Expression<Func<Domain.PhoneNumbers.PhoneNumber, PhoneNumberListItem>> To =>
        x => new PhoneNumberListItem(x.Id,
                                     x.Number,
                                     x.Type,
                                     x.ActiveAssignedPositionUser,
                                     x.ActiveAssignedPositionId);
}
