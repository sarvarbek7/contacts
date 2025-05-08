using Application.Services.Foundations;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Domain.Users;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Infrastructure.Handlers;

class UserHandler(IBaseService<User, int> userService) : IUserHandler
{
    public async Task<ErrorOr<User>> HandleAddOrGetUser(User user, CancellationToken cancellationToken = default)
    {
        var storedUser = await userService.GetAll(x => x.ExternalId == user.ExternalId)
            .FirstOrDefaultAsync(cancellationToken);

        if (storedUser is not null)
        {
            return storedUser;
        }

        return (await userService.Add(user,
                                      true,
                                      cancellationToken)).Match<ErrorOr<User>>(v => user, e => e); 
    }
}
