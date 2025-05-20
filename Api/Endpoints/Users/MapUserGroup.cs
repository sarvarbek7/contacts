namespace Contacts.Api.Endpoints.Users;

public static class MapUserGroup
{
    public static RouteGroupBuilder MapUsers(this IEndpointRouteBuilder route)
    {
        var users = route.MapGroup(Routes.Users).WithTags(Tags.Users);

        users.MapPostUsers();
        // users.MapUserByExternalId();
        users.MapSelectUser();

        return users;
    }
}