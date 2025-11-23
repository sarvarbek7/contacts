using Contacts.Api.Extensions;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.Handlers.Messages.PhoneNumbers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class RemoveUserPhoneNumberEndpoint
{
    public static RouteHandlerBuilder MapRemovePhoneNumberFromUser(this RouteGroupBuilder route)
    {
        return route.MapDelete(Routes.RemoveUserPhoneNumber, Handler);
    }

    [Authorize]
    static async Task Handler([FromRoute] Guid id,
                              [FromRoute] Guid userAssignmentId,
                              IPhoneNumberHandler handler,
                              HttpContext context)
    {
        var removePhoneNumberMessage = new RemoveUserPhoneNumberMessage(id, userAssignmentId, context.GetUserId());
        await handler.HandleRemoveUserPhoneNumber(removePhoneNumberMessage,
                                              context.RequestAborted);
    }
}