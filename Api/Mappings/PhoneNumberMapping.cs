using System.Linq.Expressions;
using Contacts.Application.Handlers.Messages.PhoneNumbers;
using Contacts.Contracts.PhoneNumbers;
using LinqKit;

namespace Contacts.Api.Mappings;

public static class PhoneNumberMapping
{
    public static CreatePhoneNumberMessage MapTo(this CreatePhoneNumberRequest request,
        int accountId)
    {
        return new CreatePhoneNumberMessage(request.Number, accountId);
    }

    public static UpdatePhoneNumberMessage MapTo(this UpdatePhoneNumberRequest request, Guid id, int accountId)
    {
        return new UpdatePhoneNumberMessage(id, request.Number, accountId);
    }

    public static ListPhoneNumbersMessage MapTo(this ListPhoneNumbersQuery query)
    {
        return new ListPhoneNumbersMessage(query.ToPagination(),
                                           query.Number,
                                           query.User,
                                           query.Status,
                                           query.UserExternalId,
                                           query.PositionId);
    }

    public static AssignUserPhoneNumberMessage MapTo(this AssignPhoneNumberRequest request,
                                                 Guid id,
                                                 int accountId)
    {
        return new(request.User.MapTo(), id, accountId);
    }

    public static AssignPositionPhoneNumberMessage MapTo(this AssignPositionPhoneNumberRequest request,
                                                 Guid id,
                                                 int accountId)
    {
        return new(request.PositionId,
                   request.Organization,
                   request.Department,
                   request.Position,
                   id,
                   accountId);
    }

    public static Expression<Func<Domain.PhoneNumbers.PhoneNumber, PhoneNumberListItem>> PhoneNumberToListItem =>
        x => new PhoneNumberListItem(x.Id,
                                     x.Number,
                                     x.ActiveAssignedUser == null ? null : UserMapping.UserDomainToListItem.Invoke(x.ActiveAssignedUser));

    public static Expression<Func<Domain.PhoneNumbers.PhoneNumber, PhoneNumber>> PhoneNumberDomainToContract =>
        x => new PhoneNumber()
        {
            Id = x.Id,
            Number = x.Number,
            ActiveAssignedUser = x.ActiveAssignedUser == null ? null : UserMapping.UserDomainToListItem.Invoke(x.ActiveAssignedUser),
            History = x.UsersHistory.Select(y => UserPhoneNumberToUserHistoryItem.Invoke(y)).ToList()
        };

    public static Expression<Func<Domain.PhoneNumbers.UserPhoneNumber, PhoneNumberHistoryItem>> UserPhoneNumberToPhoneNumberHistoryItem =>
        x => new PhoneNumberHistoryItem()
        {
            Id = x.Id,
            PhoneNumber = x.PhoneNumber == null ? null : PhoneNumberToListItem.Invoke(x.PhoneNumber),
            IsActive = x.IsActive,
            CreatedAt = x.CreatedAt,
            RemovedAt = x.RemovedAt
        };

    public static Expression<Func<Domain.PhoneNumbers.UserPhoneNumber, UserHistoryItem>> UserPhoneNumberToUserHistoryItem
     =>
        x => new UserHistoryItem()
        {
            Id = x.Id,
            User = x.User == null ? null : UserMapping.UserDomainToListItem.Invoke(x.User),
            IsActive = x.IsActive,
            CreatedAt = x.CreatedAt,
            RemovedAt = x.RemovedAt
        };
}