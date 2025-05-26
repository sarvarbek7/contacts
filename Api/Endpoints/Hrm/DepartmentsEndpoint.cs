using System.Dynamic;
using Contacts.Application.ProcessingServices;
using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.Hrm;

public static class DepartmentsEndpoint
{
    public static RouteHandlerBuilder MapDepartments(this RouteGroupBuilder route)
    {
        return route.MapGet(Routes.Departments, Handler);
    }

    static async Task<Ok<ResponseWrapper<List<Department>>>> Handler([FromServices] IHrmProClient httpClient,
                                                                     [FromQuery] int organizations,
                                                                     HttpContext httpContext)
    {
        var cancellationToken = httpContext.RequestAborted;

        var login = await httpClient.Login(cancellationToken);

        var departments = await httpClient.GetDepartments(login.TokenValue, organizations, cancellationToken);

        return TypedResults.Ok(departments);
    }
}