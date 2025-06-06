namespace Contacts.Api.Endpoints.Hrm;

public static class HrmGroup
{
    public static RouteGroupBuilder MapHrm(this IEndpointRouteBuilder route)
    {
        var hrm = route.MapGroup(Routes.BaseUrl).WithTags(Tags.HRM);

        hrm.MapStructure();
        hrm.MapDepartments();
        hrm.MapPositions();
        hrm.MapPositionsWithNumber();
        hrm.MapWorkers();
        hrm.MapWorkersWithNumber();
        hrm.MapGetAllDepartments();

        return hrm;
    }
}