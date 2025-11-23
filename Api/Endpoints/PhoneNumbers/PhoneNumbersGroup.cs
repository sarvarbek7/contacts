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
        group.MapListPhoneNumbers();
        group.MapRemovePhoneNumberFromUser();
        group.MapRemovePhoneNumberFromPosition();
        group.MapUpdatePhoneNumber();
        group.MapSelectPhoneNumbers();

        group.MapAssignPositionUserPhoneNumber();
        group.MapAssignPositionPhoneNumberBatchEndpoint();
        group.MapRemovePositionUserPhoneNumber();

        return group;
    }
}