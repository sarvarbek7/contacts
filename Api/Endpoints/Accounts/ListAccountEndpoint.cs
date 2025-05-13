using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Contracts.Accounts;
using Contracts.Common;
using LinqKit;
using Mappings;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Api.Endpoints.Accounts;

public static class ListAccountEndpoint
{
    public static RouteHandlerBuilder MapListAccounts(this RouteGroupBuilder route)
    {
        return route.MapGet(Routes.ListAccounts, Handler);
    }

    public static async Task<Results<Ok<ListResponse<Account>>, ProblemHttpResult>> Handler([AsParameters] ListAccountsQuery query,
                                                                                      [FromServices] IAccountHandler handler,
                                                                                      HttpContext context)
    {
        var message = query.MapTo();

        var result = await handler.HandleListAccount(message, context.RequestAborted);
        
        var data = await result.Query.Select(AccountMapping.AccountDomainToContract.Expand()).ToListAsync(context.RequestAborted);

        var response = data.ToListResponse(result.PageDetail);

        return TypedResults.Ok(response);
    }
}