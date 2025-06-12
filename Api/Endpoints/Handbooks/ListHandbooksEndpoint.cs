using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Contracts.Handbooks;
using Contracts.Common;
using Mappings;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.Handbooks;

public static class ListHandbooksEndpoint
{
    public static RouteHandlerBuilder MapListHandbooks(this RouteGroupBuilder route)
    {
        return route.MapGet("", Handler);
    }

    private static async Task<Ok<ListResponse<HandbookDto>>> Handler([AsParameters] ListHandbooksRequest request,
                                [FromServices] IHandbookHandler handler,
                                HttpContext context)
    {
        var message = request.MapTo();

        var result = await handler.HandleListHandbooks(message,
                                                       context.RequestAborted);

        var data = (result.Data ?? []).Select(x => x.MapTo());

        return TypedResults.Ok(data.ToListResponse(result.PageDetail));
    }
}