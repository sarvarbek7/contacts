namespace Contacts.Application.ProcessingServices.Models.Requests.HrmPro;

public record ListPositionsQuery
{
    public ListPositionsQuery(int organizationId,
                                 int? departmentId,
                                 uint? perPage,
                                 uint? page)
    {
        OrganizationId = organizationId;
        DepartmentId = departmentId;
        PerPage = perPage ?? 100;
        Page = page ?? 1;
    }

    public int OrganizationId { get; init; }
    public int? DepartmentId { get; init; }
    public uint PerPage { get; init; }
    public uint Page { get; init; }

    public string GetQueryString()
    {
        var query = $"?organization_id={OrganizationId}&per_page={PerPage}&page={Page}";

        if (DepartmentId.HasValue)
        {
            query += $"&department_id={DepartmentId}";
        }

        return query;
    }
}
