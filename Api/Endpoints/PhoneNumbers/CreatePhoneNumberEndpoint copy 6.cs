using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Contracts.PhoneNumbers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class CreatePhoneNumberEndpoint
{
    public static RouteHandlerBuilder MapCreatePhoneNumber(this RouteGroupBuilder route)
    {
        return route.MapPost(Routes.CreatePhoneNumber, Handler);
    }

    static async Task<Results<NoContent, ProblemHttpResult>> Handler([FromBody] CreatePhoneNumberRequest request,
                              HttpContext context,
                              IPhoneNumberHandler phoneNumberHandler)
    {
        var message = request.MapTo(0);

        var result = await phoneNumberHandler.HandleCreate(message, context.RequestAborted);

        if (result.IsError)
        {
            // Add to error details

            throw new NotImplementedException();
        }

        return TypedResults.NoContent();
    }
}