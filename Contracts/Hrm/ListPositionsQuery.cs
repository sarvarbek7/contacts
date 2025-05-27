using Microsoft.AspNetCore.Mvc;

namespace Contacts.Contracts.Hrm;

public record ListPositionsQuery([FromQuery(Name = "organization_id")] int OrganizationId,
                                 [FromQuery(Name = "department_id")] int? DepartmentId,
                                 [FromQuery(Name = "per_page")] uint? PerPage,
                                 [FromQuery(Name = "page")] uint? Page,
                                 [FromQuery(Name = "search")] string? Search);