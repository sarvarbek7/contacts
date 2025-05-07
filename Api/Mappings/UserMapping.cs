using System.Linq.Expressions;
using Contacts.Contracts.Users;

namespace Contacts.Api.Mappings;

public static class UserMapping
{
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

    public static Expression<Func<Domain.Users.User, User>> DomainUserToContractUser =>
        x => new User()
        {
            Id = x.Id,
            ExternalId = x.ExternalId,
            FirstName = x.FirstName,
            LastName = x.LastName,
            MiddleName = x.MiddleName,
            Photo = x.Photo
        };
}