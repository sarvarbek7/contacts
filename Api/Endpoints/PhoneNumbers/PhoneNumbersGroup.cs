namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class PhoneNumbersGroup
{
     public static RouteGroupBuilder MapPhoneNumbers(this IEndpointRouteBuilder route)
    {
        var group = route.MapGroup(Routes.PhoneNumbers).WithTags(Tags.PhoneNumbers);

        group.MapAssignPhoneNumber();
        group.MapCreatePhoneNumber();
        group.MapDeletePhoneNumberNumber();
        group.MapGetPhoneNumberNumber();
        group.MapListPhoneNumbers();
        group.MapRetainPhoneNumber();
        group.MapUpdatePhoneNumber();

        return group;
    }
}