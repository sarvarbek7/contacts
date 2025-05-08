using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.Handlers.Messages.Users;
using Contacts.Contracts.Users;
using LinqKit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.Users;

public static class GetUserByExternalIdEndpoint
{
    public static RouteHandlerBuilder MapUserByExternalId(this IEndpointRouteBuilder route)
    {
        return route.MapGet(Routes.GetByExternalId, Handler);
    }

    static async Task<Results<Ok<User>, NotFound>> Handler([FromRoute] int externalId,
                              IUserHandler userHandler,
                              HttpContext context)
    {
        var message = new GetUserByExternalIdMessage(externalId);

        var maybeUser = await userHandler.HandleGetUserByExternalId(message,
                                                                    context.RequestAborted);

        if (maybeUser is null) { 
            return TypedResults.NotFound();
        }


        return TypedResults.Ok(UserMapping.UserDomainToContract.Invoke(maybeUser));
    }
}