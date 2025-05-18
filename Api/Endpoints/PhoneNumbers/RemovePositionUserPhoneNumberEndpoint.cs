using Contacts.Api.Extensions;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.Handlers.Messages.PhoneNumbers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class RemovePositionUserPhoneNumberEndpoint
{
    public static RouteHandlerBuilder MapRemovePositionUserPhoneNumber(this RouteGroupBuilder route)
    {
        return route.MapPut(Routes.RemovePositionUserPhoneNumber, Handler);
    }

    [Authorize]
    static async Task Handler([FromRoute] Guid id,
                              IPhoneNumberHandler handler,
                              HttpContext context)
    {
        var removePhoneNumberMessage = new RemovePositionUserPhoneNumberMessage(id, context.GetUserId());

        await handler.HandleRemovePositionUserPhoneNumber(removePhoneNumberMessage,
                                              context.RequestAborted);
    }
}