using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Contracts.PhoneNumbers;
using Contracts.Common;
using LinqKit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class ListPhoneNumbersEndpoint
{
    public static RouteHandlerBuilder MapListPhoneNumbers(this RouteGroupBuilder route)
    {
        return route.MapGet(Routes.ListPhoneNumbers, Handler);
    }

    static async Task<Ok<ListResponse<PhoneNumberListItem>>> Handler([AsParameters] ListPhoneNumbersQuery query,
                        HttpContext httpContext,
                        IPhoneNumberHandler phoneNumberHandler)
    {
        var message = query.MapTo();

        var result = await phoneNumberHandler.HandleList(message, httpContext.RequestAborted);

        var data = await result.Query.Select(PhoneNumberMapping.PhoneNumberToListItem.Expand()).ToListAsync(httpContext.RequestAborted);

        var response = data.ToListResponse(result.PageDetail);

        return TypedResults.Ok(response);
    }
}