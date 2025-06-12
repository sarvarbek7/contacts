using Api.Extensions;
using Contacts.Api.Extensions;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.Handlers.Messages.Handbooks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.Handbooks;

public static class DeleteHandbookEndpoint
{
    public static RouteHandlerBuilder MapDelete(this RouteGroupBuilder route)
    {
        return route.MapDelete(Routes.WithId, Handler);
    }

    [Authorize(Roles = $"{AppRoles.Admin}, {AppRoles.SuperAdmin}")]
    private static async Task<Results<Ok, ProblemHttpResult>> Handler([FromRoute] int id,
                                                            [FromServices] IHandbookHandler handler,
                                                            HttpContext context)
    {
        var message = new DeleteHandbookMessage(id);

        var errorOrResult = await handler.HandleDeleteHandbook(message,
                                        context.RequestAborted);

        if (errorOrResult.IsError)
        {
            return TypedResults.Problem(errorOrResult.FirstError
                                          .ToProblemDetails(context.GetLanguage()));
        }

        return TypedResults.Ok();
    }
}