using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Contracts.Handbooks;
using Microsoft.AspNetCore.Mvc;
using Api.Extensions;
using Contacts.Api.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;


namespace Contacts.Api.Endpoints.Handbooks;

public static class CreateHandbookEndpoint
{
    public static RouteHandlerBuilder MapCreate(this RouteGroupBuilder route)
    {
        return route.MapPost("", Handler);
    }

    [Authorize(Roles = $"{AppRoles.Admin}, {AppRoles.SuperAdmin}")]
    private static async Task<Results<Ok<HandbookDto>, ProblemHttpResult>> Handler([FromBody] CreateHandbookRequest request,
                                [FromServices] IHandbookHandler handler,
                                HttpContext context)
    {
        var message = request.MapTo();

        var errorOrResult = await handler.HandleCreateHandbook(message, context.RequestAborted);

        if (errorOrResult.IsError)
        {
            var problemDetails = errorOrResult.FirstError.ToProblemDetails(context.GetLanguage());

            return TypedResults.Problem(problemDetails);
        }

        var handbook = errorOrResult.Value;

        return TypedResults.Ok(handbook.MapTo());
    }
}