using Contacts.Application.ProcessingServices;
using Contacts.Application.ProcessingServices.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Contacts.Api.Endpoints.Structures;

public static class ListOrganizationsEndpoint
{
    public static RouteHandlerBuilder MapListOrganizations(this RouteGroupBuilder route)
    {
        return route.MapGet(Routes.OrganizationsStructure, Handler);
    }

    async static Task<Results<Ok<List<OrganizationStructure>>, ProblemHttpResult>> Handler(HttpContext context,
    IHrmClient client)
    {
        var loginResponse = await client.Login(context.RequestAborted);

        var structure = await client.GetStructure(loginResponse.TokenValue,
                                                  context.RequestAborted);
        
        return TypedResults.Ok(structure);
    }
}