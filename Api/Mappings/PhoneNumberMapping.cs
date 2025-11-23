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
        var phoneNumberType = Enum.Parse<PhoneNumberType>(request.Type,
                                                          ignoreCase: true);

        return new CreatePhoneNumberMessage(request.Number, phoneNumberType, accountId);
    }

    public static AssignPositionUserPhoneNumberMessage MapTo(this AssignPhoneNumberToPositionUserRequest request,
                                                             Guid phoneNumberId,
                                                             Guid positionAssignmentId,
                                                             int accountId)
    {
        return new AssignPositionUserPhoneNumberMessage(request.User.MapTo(),
                                                        positionAssignmentId,
                                                        request.PositionId,
                                                        phoneNumberId,
                                                        accountId);
    }

    public static UpdatePhoneNumberMessage MapTo(this UpdatePhoneNumberRequest request, Guid id, int accountId)
    {
        var phoneNumberType = Enum.Parse<PhoneNumberType>(request.Type,
                                                          ignoreCase: true);

        return new UpdatePhoneNumberMessage(id, request.Number, phoneNumberType, accountId);
    }

    public static SearchPhoneNumbersByUserMessage MapTo(this ListPhoneNumbersByUserQuery query)
    {
        return new(query.ToPagination(), query.User);
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
        return new(request.User.MapTo(), id, request.OrganizationId, accountId);
    }

    public static AssignPositionPhoneNumberMessage MapTo(this AssignPositionPhoneNumberRequest request,
                                                 Guid id,
                                                 int accountId)
    {
        return new(request.PositionId,
                   id,
                   request.OrganizationId,
                   request.DepartmentId,
                   request.InnerPositionId,
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
             Type = x.Type.ToString(),
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


    public static SelectPhoneNumberMessage MapTo(this SelectPhoneNumbersQuery query)
      => new(query.PositionId, query.ToPagination());
}