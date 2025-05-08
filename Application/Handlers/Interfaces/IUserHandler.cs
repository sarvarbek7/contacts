using Contacts.Domain.Users;
using ErrorOr;

namespace Contacts.Application.Handlers.Interfaces;

public interface IUserHandler
{
    Task<ErrorOr<User>> HandleAddOrGetUser(User user,
                                CancellationToken cancellationToken = default);
}