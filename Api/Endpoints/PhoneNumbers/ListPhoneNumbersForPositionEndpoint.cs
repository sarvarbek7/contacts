using System.Security.Cryptography.X509Certificates;
using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.Handlers.Responses;
using Contacts.Contracts.PhoneNumbers;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class ListPhoneNumbersForPositionEndpoint
{
    public static RouteHandlerBuilder MapListPhoneNumbersForPosition(this RouteGroupBuilder route)
    {
        return route.MapGet(Routes.PhoneNumbersForPosition, Handler);
    }

    public static async Task<Results<Ok<List<WorkerWithPhoneNumber>>, ProblemHttpResult>> Handler([AsParameters] ListPhoneNumbersForPositionQuery query,
                                                                                            IPhoneNumberHandler phoneNumberHandler,
                                                                                            HttpContext httpContext)
    {
        var message = query.MapTo();

        var workers =  await phoneNumberHandler.HandlePositionPhoneNumbers(message, httpContext.RequestAborted);

        return TypedResults.Ok(workers);
    }
}