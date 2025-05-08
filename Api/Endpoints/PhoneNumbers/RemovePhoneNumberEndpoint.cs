using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.Handlers.Messages.PhoneNumbers;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class RemovePhoneNumberEndpoint
{
    public static RouteHandlerBuilder MapRetainPhoneNumber(this RouteGroupBuilder route)
    {
        return route.MapPut(Routes.RemovePhoneNumber, Handler);
    }

    static async Task Handler([FromRoute] Guid id,
                              IPhoneNumberHandler handler,
                              HttpContext context)
    {
        // TODO: fix accountId
        var removePhoneNumberMessage = new RemovePhoneNumberMessage(id, 0);

        await handler.HandleRemovePhoneNumber(removePhoneNumberMessage,
                                              context.RequestAborted);
    }
}