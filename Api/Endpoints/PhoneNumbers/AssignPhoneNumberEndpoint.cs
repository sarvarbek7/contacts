using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class AssignPhoneNumberEndpoint
{
    public static RouteHandlerBuilder MapAssignPhoneNumber(this RouteGroupBuilder route)
    {
        return route.MapPut(Routes.AssignPhoneNumber, Handler);
    }

    static Task Handler([FromRoute] Guid id) => Task.CompletedTask;
}