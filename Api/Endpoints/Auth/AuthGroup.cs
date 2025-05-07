namespace Contacts.Api.Endpoints.Auth;

public static class AuthGroup
{
    public static RouteGroupBuilder MapAuth(this IEndpointRouteBuilder route)
    {
        var auth = route.MapGroup("auth");

        return auth;
    }
}