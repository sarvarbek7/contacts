namespace Contacts.Api.Endpoints.Auth;

public static class AuthGroup
{
    public static RouteGroupBuilder MapAuth(this IEndpointRouteBuilder route)
    {
        var auth = route.MapGroup("auth")
                    .WithTags(Tags.Auth);

        auth.MapLogin();
        auth.MapCreateAccount();

        return auth;
    }
}