namespace Contacts.Api.Endpoints.Accounts;

public static class AccountGroup
{
    public static RouteGroupBuilder MapAccounts(this IEndpointRouteBuilder route)
    {
        var accounts = route.MapGroup(Routes.Accounts).WithTags(Tags.Accounts);

        accounts.MapCreateAccount();
        accounts.MapListAccounts();

        return accounts;
    } 
}