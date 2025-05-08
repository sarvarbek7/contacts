using Contacts.Application.Handlers.Messages.Users;
using Contacts.Domain.Users;
using ErrorOr;

namespace Contacts.Application.Handlers.Interfaces;

public interface IUserHandler
{
    Task<ErrorOr<User>> HandleAddOrGetUser(User user,
                                CancellationToken cancellationToken = default);
    
    // TODO: Maybe change to ErrorOr
    Task<User?> HandleGetUserByExternalId(GetUserByExternalIdMessage message, CancellationToken cancellationToken = default);
}