using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.Handlers.Messages.PhoneNumbers;
using Contacts.Contracts.PhoneNumbers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class GetPhoneNumberEndpoint
{
    public static RouteHandlerBuilder MapGetPhoneNumberNumber(this RouteGroupBuilder route)
    {
        return route.MapGet(Routes.GetPhoneNumber, Handler);
    }

    static async Task<Results<Ok<PhoneNumber>, ProblemHttpResult>> Handler([FromRoute] Guid id,
                              HttpContext context,
                              IPhoneNumberHandler phoneNumberHandler)
    {
        var message = new GetPhoneNumberByIdMessage(id);

        var result = await phoneNumberHandler.HandleGetById(message,
                                                            context.RequestAborted);

        if (result.IsError)
        {
            throw new NotImplementedException();
        }

        var dto = PhoneNumberMapping.PhoneNumberDomainToContract.Compile().Invoke(result.Value);

        return TypedResults.Ok(dto);
    }
}