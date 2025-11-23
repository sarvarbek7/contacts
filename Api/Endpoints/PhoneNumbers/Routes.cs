namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class Routes
{
    public const string PhoneNumbers = "phone_numbers";
    public const string CreatePhoneNumber = "";
    public const string UpdatePhoneNumber = "{id:guid}";
    public const string DeletePhoneNumber = "{id:guid}";
    public const string GetPhoneNumber = "{id:guid}";

    public const string PhoneNumbersForPosition = "for_position";
    

    private const string positionAssignments = "{id:guid}/position-assignments";
    private const string positionUserAssignments = $"{positionAssignments}/{{positionAssignmentId:guid}}/users";
    private const string userAssignments = "{id:guid}/user-assignments";

    public const string AssignUserPhoneNumber = userAssignments;
    public const string AssignPositionUserPhoneNumber = positionUserAssignments;
    public const string AssignPositionPhoneNumber = positionAssignments;
    public const string AssignPositionPhoneNumberBatch = "position-assignments/batch";
    public const string RemoveUserPhoneNumber = $"{userAssignments}/{{userAssignmentId:guid}}";
    public const string RemovePositionUserPhoneNumber = $"{positionUserAssignments}/{{userId:int}}";
    public const string RemovePositionPhoneNumber = $"{positionAssignments}/{{positionAssignmentId:guid}}";
    public const string ListPhoneNumbers = "";
    public const string Select = "select";
}


