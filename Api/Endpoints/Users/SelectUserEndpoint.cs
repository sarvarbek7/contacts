using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Contracts.Users;
using Contracts.Common;
using Mappings;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Api.Endpoints.Users;

public static class SelectUserEndpoint
{
    public static RouteHandlerBuilder MapSelectUser(this RouteGroupBuilder route)
    {
        return route.MapGet(Routes.Select, HandleSelect);
    }

    async static Task<Ok<ListResponse<UserListItem>>> HandleSelect([AsParameters] SelectQuery query,
                                   [FromServices] IUserHandler handler,
                                   HttpContext context)
    {
        var message = query.MapTo();

        var result = await handler.HandleSelectUsers(message, context.RequestAborted);

        var data = await result.Query!.Select(UserMapping.UserDomainToListItem)
                                      .ToListAsync(context.RequestAborted);

        var response = data.ToListResponse(result.PageDetail);

        return TypedResults.Ok(response);
    }
}