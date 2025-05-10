using Contacts.Api.Extensions;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.Handlers.Messages.PhoneNumbers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class DeletePhoneNumberEndpoint
{
    public static RouteHandlerBuilder MapDeletePhoneNumberNumber(this RouteGroupBuilder route)
    {
        return route.MapDelete(Routes.DeletePhoneNumber, Handler);
    }

    [Authorize]
    static async Task<Results<NoContent, ProblemHttpResult>> Handler([FromRoute] Guid id,
                        HttpContext context,
                        IPhoneNumberHandler phoneNumberHandler)
    {
        var message = new DeletePhoneNumberMessage(id, context.GetUserId());

        var result = await phoneNumberHandler.HandleDelete(message, context.RequestAborted);

        if (result.IsError)
        {
            // TODO: add error

            throw new NotImplementedException();
        }

        return TypedResults.NoContent();
    }
}