using Api.Extensions;
using Contacts.Api.Extensions;
using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Contracts.Handbooks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.Handbooks;

public static class UpdateHandbookEndpoint
{
    public static RouteHandlerBuilder MapUpdate(this RouteGroupBuilder route)
    {
        return route.MapPut(Routes.WithId, Handler);
    }

    [Authorize(Roles = $"{AppRoles.Admin}, {AppRoles.SuperAdmin}")]
    private static async Task<Results<Ok<HandbookDto>, ProblemHttpResult>> Handler([FromRoute] int id,
                                [FromBody] UpdateHandbookRequest request,
                                [FromServices] IHandbookHandler handler,
                                HttpContext context)
    {
        var message = request.MapTo(id);

        var errorOrResult = await handler.HandleUpdateHandbook(message,
                                                         context.RequestAborted);

        if (errorOrResult.IsError)
        {
            return TypedResults.Problem(errorOrResult.FirstError
                                          .ToProblemDetails(context.GetLanguage()));
        }

        var handbook = errorOrResult.Value;

        return TypedResults.Ok(handbook.MapTo());
    }
}