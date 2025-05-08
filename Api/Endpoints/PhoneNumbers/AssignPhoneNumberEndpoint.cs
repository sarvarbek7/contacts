using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Contracts.PhoneNumbers;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class AssignPhoneNumberEndpoint
{
    public static RouteHandlerBuilder MapAssignPhoneNumber(this RouteGroupBuilder route)
    {
        return route.MapPut(Routes.AssignPhoneNumber, Handler);
    }

    static async Task Handler([FromRoute] Guid id,
                        [FromBody] AssignPhoneNumberRequest request,
                        HttpContext context,
                        IPhoneNumberHandler handler)
    {
        // TODO add account id
        var message = request.MapTo(id, 0);

        await handler.HandleAssignPhoneNumber(message, context.RequestAborted);
    }
}