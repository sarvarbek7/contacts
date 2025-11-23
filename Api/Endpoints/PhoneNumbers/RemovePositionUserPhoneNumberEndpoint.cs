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
        return route.MapDelete(Routes.RemovePositionUserPhoneNumber, Handler);
    }

    [Authorize]
    static async Task Handler([FromRoute] Guid id,
                              [FromRoute] Guid positionAssignmentId,
                              [FromRoute] int userId,
                              IPhoneNumberHandler handler,
                              HttpContext context)
    {
        var removePhoneNumberMessage = 
            new RemovePositionUserPhoneNumberMessage(id,
                                                     positionAssignmentId,
                                                     userId,
                                                     context.GetUserId());

        await handler.HandleRemovePositionUserPhoneNumber(removePhoneNumberMessage,
                                              context.RequestAborted);
    }
}