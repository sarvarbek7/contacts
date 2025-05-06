namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class CreatePhoneNumberEndpoint
{
    public static RouteHandlerBuilder MapCreatePhoneNumber(this RouteGroupBuilder route)
    {
        return route.MapPost(Routes.CreatePhoneNumber, Handler);
    }

    static Task Handler() => Task.CompletedTask;
}