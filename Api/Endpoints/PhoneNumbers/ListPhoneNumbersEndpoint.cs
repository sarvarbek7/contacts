using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contracts.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Mappings;
using Contacts.Application.Handlers.Responses;
using Contacts.Contracts.PhoneNumbers;

namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class ListPhoneNumbersEndpoint
{
    public static RouteHandlerBuilder MapListPhoneNumbers(this RouteGroupBuilder route)
    {
        return route.MapGet(Routes.ListPhoneNumbers, Handler);
    }

    static async Task<Ok<ListResponse<PhoneNumberListItemWithPosition>>> Handler([AsParameters] ListPhoneNumbersQuery query,
                        HttpContext httpContext,
                        IPhoneNumberHandler phoneNumberHandler)
    {
        var message = query.MapTo();

        var result = await phoneNumberHandler.HandleList(message, httpContext.RequestAborted);

        var response = (result.Data ?? []).ToListResponse(result.PageDetail);

        return TypedResults.Ok(response);
    }
}