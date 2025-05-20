using Contacts.Application.Handlers.Messages.Auth;
using Contacts.Application.ProcessingServices.Models.Responses.Auth;
using Contacts.Domain.Accounts;
using ErrorOr;

namespace Contacts.Application.Handlers.Interfaces;

public interface IAuthHandler
{
    public Task<ErrorOr<LoginResponse>> HandleLogin(LoginMessage message,
                                           CancellationToken cancellationToken = default);
}