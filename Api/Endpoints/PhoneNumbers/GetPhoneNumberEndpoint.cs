namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class GetPhoneNumberEndpoint
{
    public static RouteHandlerBuilder MapGetPhoneNumberNumber(this RouteGroupBuilder route)
    {
        return route.MapGet(Routes.GetPhoneNumber, Handler);
    }

    static Task Handler() => Task.CompletedTask;
}