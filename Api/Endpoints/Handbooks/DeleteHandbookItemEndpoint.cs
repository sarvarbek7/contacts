using Api.Extensions;
using Contacts.Api.Extensions;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.Handlers.Messages.Handbooks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.Handbooks;

public static class DeleteHandbookItemEndpoint
{
    public static RouteHandlerBuilder MapDeleteHandbookItem(this RouteGroupBuilder route)
    {
        return route.MapDelete(Routes.ItemsWithItemId, Handler);
    }

    [Authorize(Roles = $"{AppRoles.Admin}, {AppRoles.SuperAdmin}")]
    private static async Task<Results<Ok, ProblemHttpResult>> Handler([FromRoute] int id,
                                                                      [FromRoute] int itemId,
                                                                      [FromServices] IHandbookHandler handler,
                                                                      HttpContext context)
    {
        var message = new DeleteHandbookItemMessage(id,
                                                    itemId);

        var result = await handler.HandleDeleteHandbookItem(message,
                                                            context.RequestAborted);

        if (result.IsError)
        {
            return TypedResults.Problem(result.FirstError.ToProblemDetails(context.GetLanguage()));
        }

        return TypedResults.Ok();
    }
}