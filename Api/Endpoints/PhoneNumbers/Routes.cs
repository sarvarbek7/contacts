namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class Routes
{
    public const string PhoneNumbers = "phone_numbers";
    public const string PhoneNumbersForPosition = "for_position";
    public const string PhoneNumbersForPositionClient = "for_position/client";
    public const string CreatePhoneNumber = "";
    public const string UpdatePhoneNumber = "{id:guid}";
    public const string DeletePhoneNumber = "{id:guid}";
    public const string GetPhoneNumber = "{id:guid}";
    public const string AssignUserPhoneNumber = "{id::guid}/assign/user";
    public const string AssignPositionUserPhoneNumber = "{id::guid}/assign/position-user";
    public const string AssignPositionPhoneNumber = "{id::guid}/assign/position";
    public const string RemoveUserPhoneNumber = "{id:guid}/remove/user";
    public const string RemovePositionUserPhoneNumber = "{id:guid}/remove/user-position";
    public const string RemovePositionPhoneNumber = "{id:guid}/remove/position";
    public const string ListPhoneNumbers = "";
    public const string SearchPhoneNumbersByUser = "by-user";
    public const string Select = "select";
}


