namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class PhoneNumbersGroup
{
     public static RouteGroupBuilder MapPhoneNumbers(this IEndpointRouteBuilder route)
    {
        var group = route.MapGroup(Routes.PhoneNumbers).WithTags(Tags.PhoneNumbers);

        group.MapAssignUserPhoneNumber();
        group.MapPositionAssignPhoneNumber();
        group.MapCreatePhoneNumber();
        group.MapDeletePhoneNumberNumber();
        group.MapGetPhoneNumberNumber();
        group.MapListPhoneNumbers();
        group.MapRemovePhoneNumberFromUser();
        group.MapRemovePhoneNumberFromPosition();
        group.MapUpdatePhoneNumber();
        group.MapListPhoneNumbersForPosition();
        group.MapSearchPhoneNumbersByUser();
        group.MapAssignPositionUserPhoneNumber();
        group.MapSelectPhoneNumbers();
        group.MapListPhoneNumbersForPositionClient();
        group.MapAssignPositionPhoneNumberBatchEndpoint();
        group.MapRemovePositionUserPhoneNumber();

        return group;
    }
}