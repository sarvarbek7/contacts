using Api.Extensions;
using Contacts.Api.Extensions;
using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Contracts.Handbooks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.Handbooks;

public static class CreateHandbookItemEndpoint
{
    public static RouteHandlerBuilder MapCreateHandbookItem(this RouteGroupBuilder route)
    {
        return route.MapPost(Routes.Items, Handler);
    }

    [Authorize(Roles = $"{AppRoles.Admin}, {AppRoles.SuperAdmin}")]
    private static async Task<Results<Ok<HandbookItemDto>, ProblemHttpResult>> Handler([FromRoute] int id,
                                                                [FromBody] CreateHandbookItemRequest request,
                                                                [FromServices] IHandbookHandler handler,
                                                                HttpContext context)
    {
        var message = request.MapTo(id);

        var errorOrResult = await handler.HandleCreateHandbookItem(message,
                                                                   context.RequestAborted);

        if (errorOrResult.IsError)
        {
            return TypedResults.Problem(errorOrResult
                    .FirstError
                    .ToProblemDetails(context.GetLanguage()));
        }

        var item = errorOrResult.Value;

        return TypedResults.Ok(item.MapTo());
    }
}