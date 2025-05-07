using Application.Common;
using Contacts.Contracts;
using Contracts.Common;

namespace Contacts.Api.Mappings;

public static class CommonMapping
{
    public static Pagination ToPagination(this PagedRecordQuery query)
    {
        return new Pagination(query.Page, query.PageSize);
    }

    public static Pagination ToPagination(this PagedClassQuery query)
    {
        return new Pagination(query.Page, query.PageSize);
    }

    public static ListResponse<T> ToListResponse<T>(this IEnumerable<T> result, PageDetail pageDetails)
    {
        return new ListResponse<T>
        {
            PageSize = pageDetails.Pagination.Take,
            Page = pageDetails.Pagination.Page,
            From = pageDetails.From,
            To = pageDetails.To,
            TotalPageCount = pageDetails.TotalPageCount,
            TotalRecords = pageDetails.TotalRecords,
            List = [.. result],
        };
    }
}