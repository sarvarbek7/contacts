namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class DeletePhoneNumberEndpoint
{
    public static RouteHandlerBuilder MapDeletePhoneNumberNumber(this RouteGroupBuilder route)
    {
        return route.MapDelete(Routes.DeletePhoneNumber, Handler);
    }

    static Task Handler() => Task.CompletedTask;
}