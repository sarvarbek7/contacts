using Contacts.Api.Extensions;
using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Contracts.PhoneNumbers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class UpdatePhoneNumberEndpoint
{
    public static RouteHandlerBuilder MapUpdatePhoneNumber(this RouteGroupBuilder route)
    {
        return route.MapPut(Routes.UpdatePhoneNumber, Handler);
    }

    [Authorize]
    static async Task<Results<NoContent, ProblemHttpResult>> Handler([FromRoute] Guid id,
                        [FromBody] UpdatePhoneNumberRequest request,
                        HttpContext context,
                        IPhoneNumberHandler phoneNumberHandler)
    {
        var message = request.MapTo(id, context.GetUserId());

        var result = await phoneNumberHandler.HandleUpdate(message, context.RequestAborted);

        if (result.IsError)
        {
            // TODO: add error
            throw new NotImplementedException();
        }

        return TypedResults.NoContent();
    }
}