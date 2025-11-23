using Api.Extensions;
using Contacts.Api.Extensions;
using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Contracts.PhoneNumbers;
using Domain.Enums;
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
                              [FromRoute] Guid positionAssignmentId,
                              [FromBody] AssignPhoneNumberToPositionUserRequest request,
                              HttpContext context,
                              IPhoneNumberHandler handler)
    {
        var message = request.MapTo(id, positionAssignmentId, context.GetUserId());

        var result = await handler.HandlePositionUserAssignPhoneNumber(message, context.RequestAborted);

        if (result.IsError)
        {
            throw new Exception();
        }
    }
}