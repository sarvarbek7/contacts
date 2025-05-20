using Api.Extensions;
using Contacts.Api.Extensions;
using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Contracts.PhoneNumbers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class CreatePhoneNumberEndpoint
{
    public static RouteHandlerBuilder MapCreatePhoneNumber(this RouteGroupBuilder route)
    {
        return route.MapPost(Routes.CreatePhoneNumber, Handler);
    }

    [Authorize]
    static async Task<Results<NoContent, ProblemHttpResult>> Handler([FromBody] CreatePhoneNumberRequest request,
                              HttpContext context,
                              IPhoneNumberHandler phoneNumberHandler)
    {
        var message = request.MapTo(context.GetUserId());

        var result = await phoneNumberHandler.HandleCreate(message, context.RequestAborted);

        if (result.IsError)
        {
            var problemDetails = result.FirstError.ToProblemDetails(context.GetLanguage());

            return TypedResults.Problem(problemDetails);
        }

        return TypedResults.NoContent();
    }
}