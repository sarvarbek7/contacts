using System.Globalization;
using Api.Extensions;
using Contacts.Api.Extensions;
using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Contracts.Handbooks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.Handbooks;

public static class UpdateHandbookItemEndpoint
{
    public static RouteHandlerBuilder MapUpdateHandbookItem(this RouteGroupBuilder route)
    {
        return route.MapPut(Routes.ItemsWithItemId, Handler);
    }

    [Authorize(Roles = $"{AppRoles.Admin}, {AppRoles.SuperAdmin}")]
    private static async Task<Results<Ok<HandbookItemDto>, ProblemHttpResult>> Handler([FromRoute] int id,
                                                                                       [FromRoute] int itemId,
                                                                                       [FromBody] UpdateHandbookItemRequest request,
                                                                                       [FromServices] IHandbookHandler handler,
                                                                                       HttpContext context)
    {
        var message = request.MapTo(id, itemId);

        var result = await handler.HandleUpdateHandbookItem(message, context.RequestAborted);

        if (result.IsError)
        {
            return TypedResults.Problem(result.FirstError.ToProblemDetails(context.GetLanguage()));
        }

        var item = result.Value;

        return TypedResults.Ok(item.MapTo());
    } 
}