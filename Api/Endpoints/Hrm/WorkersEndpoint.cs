using Contacts.Application.ProcessingServices;
using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.Hrm;

public static class WorkersEndpoint
{
    public static RouteHandlerBuilder MapWorkers(this RouteGroupBuilder route)
    {
        return route.MapGet(Routes.Workers, Handler);
    }

    static async Task<Ok<ResponseWrapper<HrmListResponse<WorkerResponse>>>> Handler([FromServices] IHrmProClient httpClient,
                                                 HttpContext httpContext)
    {
        var cancellationToken = httpContext.RequestAborted;

        var login = await httpClient.Login(cancellationToken);

        var workers = await httpClient.GetWorkers(login.TokenValue, httpContext.Request.QueryString.Value ?? "", cancellationToken);

        workers.Data.Data.ForEach(x => x.Worker.HideNumber());

        return TypedResults.Ok(workers);
    }
}