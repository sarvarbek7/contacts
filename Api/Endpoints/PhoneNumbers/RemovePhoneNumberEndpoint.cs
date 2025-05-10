using Contacts.Api.Extensions;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.Handlers.Messages.PhoneNumbers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class RemovePhoneNumberEndpoint
{
    public static RouteHandlerBuilder MapRetainPhoneNumber(this RouteGroupBuilder route)
    {
        return route.MapPut(Routes.RemovePhoneNumber, Handler);
    }

    [Authorize]
    static async Task Handler([FromRoute] Guid id,
                              IPhoneNumberHandler handler,
                              HttpContext context)
    {
        var removePhoneNumberMessage = new RemovePhoneNumberMessage(id, context.GetUserId());

        await handler.HandleRemovePhoneNumber(removePhoneNumberMessage,
                                              context.RequestAborted);
    }
}