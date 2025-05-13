using System.Dynamic;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.ProcessingServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.Hrm;

public static class PositionsWithNumberEndpoint
{
    public static RouteHandlerBuilder MapPositionsWithNumber(this RouteGroupBuilder route)
    {
        return route.MapGet(Routes.PositionsWithNumber, Handler);
    }

    static async Task<Ok<ExpandoObject>> Handler([FromServices] IHrmHandler handler,
                                                 HttpContext httpContext)
    {
        var positions = await handler.GetPositionsWithPhoneNumbers(httpContext.Request.QueryString.Value
            ?? "", httpContext.RequestAborted);

        return TypedResults.Ok(positions);
    }
}