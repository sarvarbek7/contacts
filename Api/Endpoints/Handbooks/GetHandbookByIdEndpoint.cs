using Api.Extensions;
using Contacts.Api.Extensions;
using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Contracts.Handbooks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.Handbooks;

public static class GetHandbookByIdEndpoint
{
    public static RouteHandlerBuilder MapGetHandbookById(this RouteGroupBuilder route)
    {
        return route.MapGet(Routes.WithId, Handler);
    }

    private static async Task<Results<Ok<HandbookDto>, ProblemHttpResult>> Handler([FromRoute] int id,
                                                                             [FromServices] IHandbookHandler handler,
                                                                             HttpContext context)
    {
        var result = await handler.HandleGetHandleById(id,
                                                       context.RequestAborted);

        if (result.IsError)
        {
            return TypedResults.Problem(result.FirstError
                    .ToProblemDetails(context.GetLanguage()));
        }

        var handbook = result.Value;

        return TypedResults.Ok(handbook.MapTo());
    }
}