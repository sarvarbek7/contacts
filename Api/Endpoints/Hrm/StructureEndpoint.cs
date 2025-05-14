using System.Dynamic;
using Contacts.Application.ProcessingServices;
using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.Hrm;

public static class StructureEndpoint
{
    public static RouteHandlerBuilder MapStructure(this RouteGroupBuilder route)
    {
        return route.MapGet(Routes.Structure, Handler);
    }

    static async Task<Ok<ResponseWrapper<List<Organization>>>> Handler([FromServices] IHrmProClient httpClient,
                                                 HttpContext context)
    {
        var ct = context.RequestAborted;

        var login = await httpClient.Login(ct);

        var structure = await httpClient.GetStructure(login.TokenValue, ct);

        return TypedResults.Ok(structure);
    }
}