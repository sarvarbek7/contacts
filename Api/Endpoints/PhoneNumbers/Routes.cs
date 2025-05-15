namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class Routes
{
    public const string PhoneNumbers = "phone_numbers";

    public const string PhoneNumbersForPosition = "phone_numbers/for_position";

    public const string CreatePhoneNumber = "";
    public const string UpdatePhoneNumber = "{id:guid}";
    public const string DeletePhoneNumber = "{id:guid}";
    public const string GetPhoneNumber ="{id:guid}";
    public const string AssignUserPhoneNumber = "{id::guid}/assign/user";
    public const string AssignPositionPhoneNumber = "{id::guid}/assign/position";
    public const string RemoveUserPhoneNumber = "{id:guid}/remove/user";
    public const string RemovePositionPhoneNumber = "{id:guid}/remove/position";
    public const string ListPhoneNumbers = "";
}


