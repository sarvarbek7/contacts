namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class UpdatePhoneNumberEndpoint
{
    public static RouteHandlerBuilder MapUpdatePhoneNumber(this RouteGroupBuilder route)
    {
        return route.MapPut(Routes.UpdatePhoneNumber, Handler);
    }

    static Task Handler() => Task.CompletedTask;
}