using Contacts.Api.Extensions;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.Handlers.Messages.PhoneNumbers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class RemovePositionPhoneNumberEndpoint
{
    public static RouteHandlerBuilder MapRemovePhoneNumberFromPosition(this RouteGroupBuilder route)
    {
        return route.MapPut(Routes.RemovePositionPhoneNumber, Handler);
    }

    [Authorize]
    static async Task Handler([FromRoute] Guid id,
                              IPhoneNumberHandler handler,
                              HttpContext context)
    {
        var removePhoneNumberMessage = new RemovePositionPhoneNumberMessage(id, context.GetUserId());

        await handler.HandleRemovePhoneNumberFromPosition(removePhoneNumberMessage,
                                              context.RequestAborted);
    }
}