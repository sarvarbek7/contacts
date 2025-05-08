using Application.Services.Foundations;
using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Api.Endpoints.Users;

public static class PostUserEndpoint
{
    public static RouteHandlerBuilder MapPostUsers(this RouteGroupBuilder route)
    {
        return route.MapPost(Routes.Users, Handler);
    }

    static async Task Handler([FromBody] Contracts.Users.HrmUser user,
       [FromServices] IUserHandler handler,
       [FromServices] HttpContext httpContext)
    {
        var domainUser = user.MapTo();

        await handler.HandleAddOrGetUser(domainUser, httpContext.RequestAborted);
    }
}