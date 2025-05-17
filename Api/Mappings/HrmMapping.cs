using Contacts.Application.ProcessingServices.Models.Requests.HrmPro;

namespace Contacts.Api.Mappings;

public static class HrmMapping
{
    public static ListPositionsQuery MapTo(this Contracts.Hrm.ListPositionsQuery query)
    {
        return new(query.OrganizationId,
                   query.DepartmentId,
                   query.PerPage,
                   query.Page);
    }
}