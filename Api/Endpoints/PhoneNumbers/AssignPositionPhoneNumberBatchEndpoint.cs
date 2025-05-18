using Application.Services.Foundations;
using Contacts.Api.Extensions;
using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Contracts.PhoneNumbers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class AssignPositionPhoneNumberBatchEndpoint
{
    public static RouteHandlerBuilder MapAssignPositionPhoneNumberBatchEndpoint(this RouteGroupBuilder route)
    {
        return route.MapPut(Routes.AssignPositionPhoneNumberBatch, Handler);
    }

    [Authorize]
    static async Task Handler([FromBody] AssignPositionPhoneNumberBatchRequest request,
                        HttpContext context,
                        IPhoneNumberHandler handler,
                        IBaseService<Domain.PhoneNumbers.PhoneNumber, Guid> service)
    {
        try
        {
            await service.StartTransaction(context.RequestAborted);

            foreach (var id in request.PhoneIds)
            {
                var message = request.MapTo(id, context.GetUserId());

                await handler.HandlePositionAssignPhoneNumber(message, context.RequestAborted);
            }
        }
        catch
        {
            await service.RollbackTransaction();
        }
        finally
        {
            await service.CommitTransaction(context.RequestAborted);
        }
    }
}