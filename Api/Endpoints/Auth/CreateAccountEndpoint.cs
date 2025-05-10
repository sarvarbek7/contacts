using Contacts.Api.Extensions;
using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Contracts.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.Auth;

public static class CreateAccountEndpoint
{
    public static RouteHandlerBuilder MapCreateAccount(this IEndpointRouteBuilder route)
    {
        return route.MapPost(Routes.CreateAccount, Handler);
    }

    [Authorize(Roles = "SuperAdmin")]
    private static async Task Handler([FromBody] CreateAccountRequest request,
                                [FromServices] IAuthHandler authHandler,
                                HttpContext context)
    {
        var message = request.MapTo(context.GetUserId());
        
        await authHandler.HandleCreateAccount(message, context.RequestAborted);
    }
}