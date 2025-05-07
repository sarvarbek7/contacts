namespace Contacts.Api.Endpoints.PhoneNumbers;

public static class Routes
{
    public const string PhoneNumbers = "phone_numbers";

    public const string CreatePhoneNumber = "";
    public const string UpdatePhoneNumber = "{id:guid}";
    public const string DeletePhoneNumber = "{id:guid}";
    public const string GetPhoneNumber ="{id:guid}";
    public const string AssignPhoneNumber = "{id::guid}/assign";
    public const string RetainPhoneNumber = "{id:guid}/retain";
    public const string ListPhoneNumbers = "";
}


