using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contracts.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Mappings;
using Contacts.Contracts.PhoneNumbers;

namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class SearchPhoneNumbersByUserEndpoint
{
    public static RouteHandlerBuilder MapSearchPhoneNumbersByUser(this RouteGroupBuilder route)
    {
        return route.MapGet(Routes.SearchPhoneNumbersByUser, Handler);
    }

    static async Task<Ok<ListResponse<PhoneNumberListItemSearchUser>>> Handler([AsParameters] ListPhoneNumbersByUserQuery query,
                        HttpContext httpContext,
                        IPhoneNumberHandler phoneNumberHandler)
    {
        var message = query.MapTo();

        var result = await phoneNumberHandler.HandleSearchByUser(message, httpContext.RequestAborted);

        var response = (result.Data ?? []).Select(PhoneNumberMapping.PhoneNumberListItemToContractItemFunc).ToListResponse(result.PageDetail);

        return TypedResults.Ok(response);
    }
}