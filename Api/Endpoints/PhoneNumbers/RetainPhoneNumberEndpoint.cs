using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class RetainPhoneNumberEndpoint
{
    public static RouteHandlerBuilder MapRetainPhoneNumber(this RouteGroupBuilder route)
    {
        return route.MapPut(Routes.RetainPhoneNumber, Handler);
    }

    static Task Handler([FromRoute] Guid id) => Task.CompletedTask;
}