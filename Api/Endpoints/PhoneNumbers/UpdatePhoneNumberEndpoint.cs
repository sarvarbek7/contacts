using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Contracts.PhoneNumbers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class UpdatePhoneNumberEndpoint
{
    public static RouteHandlerBuilder MapUpdatePhoneNumber(this RouteGroupBuilder route)
    {
        return route.MapPut(Routes.UpdatePhoneNumber, Handler);
    }

    static async Task<Results<NoContent, ProblemHttpResult>> Handler([FromRoute] Guid id,
                        [FromBody] UpdatePhoneNumberRequest request,
                        HttpContext context,
                        IPhoneNumberHandler phoneNumberHandler)
    {
        var message = request.MapTo(id, 0);

        var result = await phoneNumberHandler.HandleUpdate(message, context.RequestAborted);

        if (result.IsError)
        {
            // TODO: add error
            throw new NotImplementedException();
        }

        return TypedResults.NoContent();
    }
}