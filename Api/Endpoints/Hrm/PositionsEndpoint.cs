using Contacts.Application.ProcessingServices;
using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;
using Contacts.Api.Mappings;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.Hrm;

public static class PositionsEndpoint
{
    public static RouteHandlerBuilder MapPositions(this RouteGroupBuilder route)
    {
        return route.MapGet(Routes.Positions, Handler);
    }

    static async Task<Ok<ResponseWrapper<HrmListResponse<Position>>>> Handler([FromServices] IHrmProClient httpClient,
                                                                              [AsParameters] Contracts.Hrm.ListPositionsQuery query,
                                                                              HttpContext httpContext)
    {
        var cancellationToken = httpContext.RequestAborted;

        var login = await httpClient.Login(cancellationToken);

        var hrmQuery = query.MapTo();

        var positions = await httpClient.GetPositions(login.TokenValue, hrmQuery, cancellationToken);

        return TypedResults.Ok(positions);
    }
}