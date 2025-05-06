namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class ListPhoneNumbersEndpoint
{
    public static RouteHandlerBuilder MapListPhoneNumbers(this RouteGroupBuilder route)
    {
        return route.MapGet(Routes.ListPhoneNumbers, Handler);
    }

    static Task Handler() => Task.CompletedTask;
}