using System.Linq.Expressions;
using Contacts.Application.Handlers.Messages.Users;
using Contacts.Contracts.Users;
using Mappings;

namespace Contacts.Api.Mappings;

public static class UserMapping
{
    public static SelectUsersMessage MapTo(this SelectQuery query)
        => new (query.Search, query.HaveNumber, query.ToPagination());

    public static Domain.Users.User MapTo(this HrmUser dto)
    {
        return new Domain.Users.User()
        {
            ExternalId = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            MiddleName = dto.MiddleName,
            Photo = dto.Photo,
        };
    }

    public static Expression<Func<Domain.Users.User, UserListItem>> UserDomainToListItem =>
        x => new UserListItem()
        {
            Id = x.Id,
            ExternalId = x.ExternalId,
            FirstName = x.FirstName,
            LastName = x.LastName,
            MiddleName = x.MiddleName,
            Photo = x.Photo,
        };
    
    // public static Expression<Func<Domain.Users.User, User>> UserDomainToContract =>
    //     x => new User()
    //     {
    //         Id = x.Id,
    //         ExternalId = x.ExternalId,
    //         FirstName = x.FirstName,
    //         LastName = x.LastName,
    //         MiddleName = x.MiddleName,
    //         Photo = x.Photo,
    //         History = x.PhoneNumberHistory.Select(PhoneNumberMapping.UserPhoneNumberToPhoneNumberHistoryItem.Invoke).ToList(),
    //         ActivePhoneNumbers = x.ActivePhoneNumbers.Select(PhoneNumberMapping.PhoneNumberToListItem.Invoke).ToList()
    //     };
}