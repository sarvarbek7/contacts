using Application.Common;
using Contacts.Application.Handlers.Messages.Users;
using Contacts.Domain.Users;
using ErrorOr;

namespace Contacts.Application.Handlers.Interfaces;

public interface IUserHandler
{
    Task<ErrorOr<User>> HandleAddOrGetUser(User user,
                                CancellationToken cancellationToken = default);

    Task<User?> HandleGetUserByExternalId(GetUserByExternalIdMessage message, CancellationToken cancellationToken = default);

    Task<ListResult<User>> HandleSelectUsers(SelectUsersMessage message,
                                             CancellationToken cancellationToken = default);
}