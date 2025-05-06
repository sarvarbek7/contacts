namespace Contacts.Api.Endpoints.Structures;

public static class OrganizationStructureGroup
{
    public static RouteGroupBuilder MapOrganizationStructure(this IEndpointRouteBuilder route)
    {
        var group = route.MapGroup(Routes.Structure);

        group.MapListOrganizations();

        return group;
    }
}