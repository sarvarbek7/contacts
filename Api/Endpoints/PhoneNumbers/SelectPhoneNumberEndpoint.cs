using Application.Common;
using Application.Common.Extensions;
using Application.Services.Foundations;
using Contacts.Api.Mappings;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Contracts.PhoneNumbers;
using Contracts.Common;
using Mappings;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class SelectPhoneNumberEndpoint
{
    public static RouteHandlerBuilder MapSelectPhoneNumbers(this RouteGroupBuilder route)
    {
        return route.MapGet(Routes.Select, Handler);
    }

    static async Task<Ok<ListResponse<SelectPhoneNumber>>> Handler([AsParameters] SelectPhoneNumbersQuery query,
                              [FromServices] IBaseService<Domain.PhoneNumbers.PhoneNumber, Guid> service,
                              HttpContext context)
    {
        var queryable = service.GetAll(tracked: false);

        if (query.PositionId.HasValue)
        {
            queryable = queryable.Where(p => p.ActiveAssignedPositionId == query.PositionId);
        }

        if (query.Search?.Trim() is { } search)
        {
            queryable = queryable.Where(x => x.Number.Contains(search));
        }

        int total = await queryable.CountAsync(context.RequestAborted);

        var pagination = query.ToPagination();

        queryable = queryable.OrderBy(x => x.Number);

        queryable = queryable.Paged(pagination);

        var data = (await queryable.Select(x => new { x.Id, x.Number, x.Type, x.ActiveAssignedPositionUser })
            .ToListAsync(context.RequestAborted))
            .Select(x => new  SelectPhoneNumber(x.Id,
                                                x.Number,
                                                x.Type.ToString().ToLower(),
                                                x.ActiveAssignedPositionUser is null ? null : UserMapping.UserDomainToListItem.Compile().Invoke(x.ActiveAssignedPositionUser)));

        PageDetail pageDetail = new(pagination, total);

        return TypedResults.Ok(data.ToListResponse(pageDetail));
    }
}