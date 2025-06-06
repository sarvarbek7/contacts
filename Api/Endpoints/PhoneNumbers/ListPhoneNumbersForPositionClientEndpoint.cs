using System.Security.Cryptography.X509Certificates;
using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.Handlers.Responses;
using Contacts.Contracts.PhoneNumbers;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class ListPhoneNumbersForPositionClientEndpoint
{
    public static RouteHandlerBuilder MapListPhoneNumbersForPositionClient(this RouteGroupBuilder route)
    {
        return route.MapGet(Routes.PhoneNumbersForPositionClient, Handler);
    }

    public static async Task<Results<Ok<List<WorkerWithPhoneNumber>>, ProblemHttpResult>> Handler([AsParameters] ListPhoneNumbersForPositionQuery query,
                                                                                            IPhoneNumberHandler phoneNumberHandler,
                                                                                            HttpContext httpContext)
    {
        var message = query.MapToClientMessage();

        var workers =  await phoneNumberHandler.HandlePositionPhoneNumbersClient(message, httpContext.RequestAborted);

        workers.ForEach(x => x.Worker.HideNumber());

        return TypedResults.Ok(workers);
    }
}