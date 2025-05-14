using System.Dynamic;
using Contacts.Application.ProcessingServices;
using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.Hrm;

public static class PositionsEndpoint
{
    public static RouteHandlerBuilder MapPositions(this RouteGroupBuilder route)
    {
        return route.MapGet(Routes.Positions, Handler);
    }

    static async Task<Ok<ResponseWrapper<ListResponse<Position>>>> Handler([FromServices] IHrmProClient httpClient,
                                                 HttpContext httpContext)
    {
        var cancellationToken = httpContext.RequestAborted;

        var login = await httpClient.Login(cancellationToken);

        var positions = await httpClient.GetPositions(login.TokenValue, httpContext.Request.QueryString.Value ?? "", cancellationToken);

        return TypedResults.Ok(positions);
    }
}