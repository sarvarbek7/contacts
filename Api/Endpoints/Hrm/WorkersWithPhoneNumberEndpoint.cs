using System.Dynamic;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.Handlers.Responses;
using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.Hrm;

public static class WorkersWithPhoneNumberEndpoint
{
    public static RouteHandlerBuilder MapWorkersWithNumber(this RouteGroupBuilder route)
    {
        return route.MapGet(Routes.WorkersWithNumber, Handler);
    }

    static async Task<Ok<ResponseWrapper<ListResponse<WorkerWithPhoneNumber>>>> Handler([FromServices] IHrmHandler handler,
                                                 HttpContext httpContext)
    {
        var cancellationToken = httpContext.RequestAborted;

        var workers = await handler.GetWorkersWithPhoneNumbers(httpContext.Request.QueryString.Value ?? "", cancellationToken);

        return TypedResults.Ok(workers);
    }
}