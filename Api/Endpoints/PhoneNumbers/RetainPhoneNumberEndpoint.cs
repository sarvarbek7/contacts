namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class RetainPhoneNumberEndpoint
{
    public static RouteHandlerBuilder MapRetainPhoneNumber(this RouteGroupBuilder route)
    {
        return route.MapPut(Routes.RetainPhoneNumber, Handler);
    }

    static Task Handler() => Task.CompletedTask;
}