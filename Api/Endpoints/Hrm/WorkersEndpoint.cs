using System.Dynamic;
using Contacts.Application.ProcessingServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.Hrm;

public static class WorkersEndpoint
{
    public static RouteHandlerBuilder MapWorkers(this RouteGroupBuilder route)
    {
        return route.MapGet(Routes.Workers, Handler);
    }

    static async Task<Ok<ExpandoObject>> Handler([FromServices] IHrmProClient httpClient,
                                                 HttpContext httpContext)
    {
        var cancellationToken = httpContext.RequestAborted;

        var login = await httpClient.Login(cancellationToken);

        var workers = await httpClient.GetWorkers(login.TokenValue, httpContext.Request.QueryString.Value ?? "", cancellationToken);

        return TypedResults.Ok(workers);
    }
}