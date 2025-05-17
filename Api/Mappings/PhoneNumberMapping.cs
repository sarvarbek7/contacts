using System.Linq.Expressions;
using Contacts.Application.Handlers.Messages.PhoneNumbers;
using Contacts.Contracts.PhoneNumbers;
using Contacts.Domain.PhoneNumbers;
using LinqKit;
using Mappings;

namespace Contacts.Api.Mappings;

public static class PhoneNumberMapping
{
    public static CreatePhoneNumberMessage MapTo(this CreatePhoneNumberRequest request,
        int accountId)
    {
        return new CreatePhoneNumberMessage(request.Number, accountId);
    }

    public static AssignPositionUserPhoneNumberMessage MapTo(this AssignPhoneNumberToPositionUserRequest request,
                                                             Guid phoneNumberId,
                                                             int accountId)
    {
        return new AssignPositionUserPhoneNumberMessage(request.User.MapTo(),
                                                        request.PositionId,
                                                        phoneNumberId,
                                                        accountId);
    }

    public static UpdatePhoneNumberMessage MapTo(this UpdatePhoneNumberRequest request, Guid id, int accountId)
    {
        return new UpdatePhoneNumberMessage(id, request.Number, accountId);
    }

    public static SearchPhoneNumbersByUserMessage MapTo(this ListPhoneNumbersByUserQuery query)
    {
        return new(query.ToPagination(), query.User);
    }

    public static ListPhoneNumbersMessage MapTo(this ListPhoneNumbersQuery query)
    {
        List<int> positions = [];

        if (query.Positions is { } positionsQuery)
        {
            positions.AddRange(positionsQuery.Split(',').Select(int.Parse));
        }

        return new ListPhoneNumbersMessage(query.ToPagination(),
                                           query.Number,
                                           query.User,
                                           query.Status,
                                           query.UserExternalId,
                                           query.PositionId,
                                           positions);
    }

    public static AssignUserPhoneNumberMessage MapTo(this AssignPhoneNumberRequest request,
                                                 Guid id,
                                                 int accountId)
    {
        return new(request.User.MapTo(), id, request.OrganizationId, accountId);
    }

    public static AssignPositionPhoneNumberMessage MapTo(this AssignPositionPhoneNumberRequest request,
                                                 Guid id,
                                                 int accountId)
    {
        return new(request.PositionId,
                   id,
                   request.OrganizationId,
                   request.Organization,
                   request.Department,
                   request.Position,
                   accountId);
    }

    // public static Expression<Func<Domain.PhoneNumbers.PhoneNumber, PhoneNumberListItem>> PhoneNumberToListItem =>
    //     x => new PhoneNumberListItem(x.Id,
    //                                  x.Number,
    //                                  x.ActiveAssignedUser == null ? null : UserMapping.UserDomainToListItem.Invoke(x.ActiveAssignedUser),
    //                                  x.ActiveAssignedPositionId);

    public static Func<Application.Handlers.Responses.PhoneNumberListItem, PhoneNumberListItemSearchUser> PhoneNumberListItemToContractItemFunc =>
         x => new PhoneNumberListItemSearchUser
         {
             Id = x.Id,
             Number = x.Number,
             AssignedUser = UserMapping.UserDomainToListItem!.Invoke(x.AssignedUser),
         };

    public static Expression<Func<Domain.PhoneNumbers.PhoneNumber, Contracts.PhoneNumbers.PhoneNumber>> PhoneNumberDomainToContract =>
        x => new Contracts.PhoneNumbers.PhoneNumber()
        {
            Id = x.Id,
            Number = x.Number,
            ActiveAssignedUser = x.ActiveAssignedUser == null ? null : UserMapping.UserDomainToListItem.Invoke(x.ActiveAssignedUser),
            History = x.UsersHistory.Select(u => UserPhoneNumberToUserHistoryItem.Invoke(u)).ToList(),
            PositionHistory = x.PositionHistory.Select(p => UserPositionPhoneNumberToHistoryItem.Invoke(p)).ToList()
        };

    // public static Expression<Func<UserPhoneNumber, PhoneNumberHistoryItem>> UserPhoneNumberToPhoneNumberHistoryItem =>
    //     x => new PhoneNumberHistoryItem()
    //     {
    //         Id = x.Id,
    //         PhoneNumber = x.PhoneNumber == null ? null : PhoneNumberToListItem.Invoke(x.PhoneNumber),
    //         IsActive = x.IsActive,
    //         CreatedAt = x.CreatedAt,
    //         RemovedAt = x.RemovedAt
    //     };

    public static Expression<Func<UserPhoneNumber, UserHistoryItem>> UserPhoneNumberToUserHistoryItem
     =>
        x => new UserHistoryItem()
        {
            Id = x.Id,
            User = x.User == null ? null : UserMapping.UserDomainToListItem.Invoke(x.User),
            IsActive = x.IsActive,
            CreatedAt = x.CreatedAt,
            RemovedAt = x.RemovedAt
        };

    public static Expression<Func<PositionPhoneNumber, PositionHistoryItem>> UserPositionPhoneNumberToHistoryItem =>
        x => new PositionHistoryItem()
        {
            Id = x.Id,
            PositionId = x.PositionId,
            IsActive = x.IsActive,
            CreatedAt = x.CreatedAt,
            RemovedAt = x.RemovedAt,
        };

    public static ListPhoneNumbersForPositionMessage MapTo(this ListPhoneNumbersForPositionQuery query)
        => new(query.OrganizationId, query.PositionId);
        
     public static ListPhoneNumbersForPositionMessageClient MapToClientMessage(this ListPhoneNumbersForPositionQuery query)
        => new (query.OrganizationId, query.PositionId);
}