namespace Contacts.Api.Endpoints.Handbooks;

public static class HandbookGroup
{
    public static RouteGroupBuilder MapHandbooks(this IEndpointRouteBuilder route)
    {
        var handbooks = route.MapGroup(Routes.BaseUrl)
                             .WithTags(Tags.Handbooks);

        handbooks.MapCreate();
        handbooks.MapUpdate();
        handbooks.MapDelete();
        handbooks.MapGetHandbookById();
        handbooks.MapListHandbooks();

        handbooks.MapCreateHandbookItem();
        handbooks.MapUpdateHandbookItem();
        handbooks.MapDeleteHandbookItem();

        return handbooks;
    }
}