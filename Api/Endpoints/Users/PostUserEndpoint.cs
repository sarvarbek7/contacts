using Application.Services.Foundations;
using Contacts.Api.Mappings;
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
       [FromServices] IBaseService<User, int> userService,
       [FromServices] HttpContext httpContext)
    {
        bool userExists = await userService.GetAll(x => x.ExternalId == user.Id)
            .AnyAsync(httpContext.RequestAborted);

        if (userExists)
        {
            return;
        }

        var domainUser = user.MapTo();

        await userService.Add(domainUser, true, httpContext.RequestAborted);
    }
}