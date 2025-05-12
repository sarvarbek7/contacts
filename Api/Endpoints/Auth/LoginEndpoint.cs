using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Contracts.Auth;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.Auth;

public static class LoginEndpoint
{
    public static RouteHandlerBuilder MapLogin(this IEndpointRouteBuilder endpoint)
    {
        return endpoint.MapPost(Routes.Login, Handler);
    }

    private static async Task<Results<Ok<LoginResponse>, ProblemHttpResult>> Handler([FromBody] LoginRequest request,
        [FromServices] IAuthHandler authHandler,
        HttpContext context)
    {
        var message = request.MapTo();

        var result = await authHandler.HandleLogin(message,
            context.RequestAborted);


        return TypedResults.Ok(result.MapTo());
    }
}