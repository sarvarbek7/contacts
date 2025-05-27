using Application.ProcessingServices;

namespace Contacts.Application.ProcessingServices.Models.Requests.HrmPro;

public record ListPositionsQuery
{
    public ListPositionsQuery(int organizationId,
                                 int? departmentId,
                                 string? search,
                                 uint? perPage,
                                 uint? page)
    {
        OrganizationId = organizationId;
        DepartmentId = departmentId;
        PerPage = perPage ?? 100;
        Page = page ?? 1;
        Search = search;
    }

    public int OrganizationId { get; init; }
    public int? DepartmentId { get; init; }
    public string? Search { get; init; }
    public uint PerPage { get; init; }
    public uint Page { get; init; }

    public string GetQueryString(ITranslationService translationService)
    {
        var query = $"?organization_id={OrganizationId}&per_page={PerPage}&page={Page}";

        if (DepartmentId.HasValue)
        {
            query += $"&department_id={DepartmentId}";
        }

        if (Search?.Trim().ToLower() is { Length: > 0 } search)
        {
            var translationResult = translationService.Translate(search);

            if (translationResult?.Latin is { Length: > 0 } latin)
            {
                query += $"search={latin}";
            }
        }

        return query;
    }
}
