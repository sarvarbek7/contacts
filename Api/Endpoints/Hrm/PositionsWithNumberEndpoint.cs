using System.Dynamic;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.Handlers.Responses;
using Contacts.Application.ProcessingServices;
using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.Hrm;

public static class PositionsWithNumberEndpoint
{
    public static RouteHandlerBuilder MapPositionsWithNumber(this RouteGroupBuilder route)
    {
        return route.MapGet(Routes.PositionsWithNumber, Handler);
    }

    static async Task<Ok<ResponseWrapper<HrmListResponse<PositionWithPhoneNumber>>>> Handler([FromServices] IHrmHandler handler,
                                                 HttpContext httpContext)
    {
        var positions = await handler.GetPositionsWithPhoneNumbers(httpContext.Request.QueryString.Value
            ?? "", httpContext.RequestAborted);

        return TypedResults.Ok(positions);
    }
}