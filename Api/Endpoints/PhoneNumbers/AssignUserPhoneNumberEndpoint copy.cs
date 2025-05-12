using Contacts.Api.Extensions;
using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Contracts.PhoneNumbers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class AssignUserPhoneNumberEndpoint
{
    public static RouteHandlerBuilder MapAssignUserPhoneNumber(this RouteGroupBuilder route)
    {
        return route.MapPut(Routes.AssignUserPhoneNumber, Handler);
    }

    [Authorize]
    static async Task Handler([FromRoute] Guid id,
                        [FromBody] AssignPhoneNumberRequest request,
                        HttpContext context,
                        IPhoneNumberHandler handler)
    {
        var message = request.MapTo(id, context.GetUserId());

        await handler.HandleUserAssignPhoneNumber(message, context.RequestAborted);
    }
}