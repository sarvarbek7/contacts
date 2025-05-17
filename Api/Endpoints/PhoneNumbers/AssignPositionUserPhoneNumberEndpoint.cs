using Contacts.Api.Extensions;
using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Contracts.PhoneNumbers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class AssignPositionUserPhoneNumberEndpoint
{
    public static RouteHandlerBuilder MapAssignPositionUserPhoneNumber(this RouteGroupBuilder route)
    {
        return route.MapPut(Routes.AssignPositionUserPhoneNumber, Handler);
    }

    [Authorize]
    static async Task Handler([FromRoute] Guid id,
                              [FromBody] AssignPhoneNumberToPositionUserRequest request,
                              HttpContext context,
                              IPhoneNumberHandler handler)
    {
        var message = request.MapTo(id, context.GetUserId());

        await handler.HandlePositionUserAssignPhoneNumber(message, context.RequestAborted);
    }
}