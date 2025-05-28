using Contacts.Application.ProcessingServices;
using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.Hrm;

public static class GetAllDepartmentsEndpoint
{
    public static RouteHandlerBuilder MapGetAllDepartments(this RouteGroupBuilder route)
    {
        return route.MapGet(Routes.GetAllDepartments, Handler);
    }

    private async static Task<Ok<ResponseWrapper<HrmListResponse<DepartmentSearchItem>>>> Handler([FromServices] IHrmProClient httpClient,
                                HttpContext httpContext)
    {
        var cancellationToken = httpContext.RequestAborted;

        var login = await httpClient.Login(cancellationToken);

        var departments = await httpClient.GetSearchDepartments(login.TokenValue,
                                                                httpContext.Request.QueryString.Value ?? "",
            cancellationToken);

        return TypedResults.Ok(departments);
    }
}