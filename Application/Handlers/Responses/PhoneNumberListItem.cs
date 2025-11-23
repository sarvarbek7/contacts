using System.Linq.Expressions;
using Contacts.Domain.PhoneNumbers;

namespace Contacts.Application.Handlers.Responses;


public record PhoneNumberListItem(Guid Id,
                                  string Number,
                                  PhoneNumberType Type,
                                  IEnumerable<PositionWithUserExternalIds> PositionWithExternalIds)
{
       public static Expression<Func<PhoneNumber, PhoneNumberListItem>> To =>
              x => new PhoneNumberListItem(x.Id,
                                           x.Number,
                                           x.Type,
                                           x.AssignedPositions.Select(pa => new PositionWithUserExternalIds(
                                                 pa.Id,
                                                 pa.PositionId,
                                                 pa.Users.Select(u => u.ExternalId)
                                           )));
}

public record PositionWithUserExternalIds(Guid PositionAssignmentId, int PositionId, IEnumerable<int> WorkerExternalIds);
