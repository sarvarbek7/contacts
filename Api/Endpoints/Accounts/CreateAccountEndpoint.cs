using Contacts.Api.Extensions;
using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Contracts.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.Accounts;

public static class CreateAccountEndpoint
{
    public static RouteHandlerBuilder MapCreateAccount(this RouteGroupBuilder route)
    {
        return route.MapPost(Routes.CreateAccount, Handler);
    }

    [Authorize(Roles = "SuperAdmin")]
    private static async Task Handler([FromBody] CreateAccountRequest request,
                                [FromServices] IAccountHandler authHandler,
                                HttpContext context)
    {
        var message = request.MapTo(context.GetUserId());
        
        await authHandler.HandleCreateAccount(message, context.RequestAborted);
    }
}