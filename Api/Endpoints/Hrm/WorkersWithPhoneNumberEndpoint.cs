using System.Dynamic;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.ProcessingServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.Hrm;

public static class WorkersWithPhoneNumberEndpoint
{
    public static RouteHandlerBuilder MapWorkersWithNumber(this RouteGroupBuilder route)
    {
        return route.MapGet(Routes.WorkersWithNumber, Handler);
    }

    static async Task<Ok<ExpandoObject>> Handler([FromServices] IHrmHandler handler,
                                                 HttpContext httpContext)
    {
        var cancellationToken = httpContext.RequestAborted;

        var workers = await handler.GetWorkersWithPhoneNumbers(httpContext.Request.QueryString.Value ?? "", cancellationToken);

        return TypedResults.Ok(workers);
    }
}