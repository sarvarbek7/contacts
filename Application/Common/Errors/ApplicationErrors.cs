using ErrorOr;

namespace Contacts.Application.Common.Errors;

public static class ApplicationErrors
{
    public static readonly string errorPrefix = "ApplicationErrors";

    public static Error ExternalServerError => Error.Failure($"{errorPrefix}.ExternalServerError");

    public static Error UserNotFoundInHRMSystem => Error.Validation($"{errorPrefix}.UserNotFoundInHRMSystem");
    public static Error PhoneNumberAlreadyExists => Error.Conflict($"{errorPrefix}.PhoneNumberAlreadyExists");
    public static Error InvalidRailwayPhoneNumber => Error.Validation($"{errorPrefix}.InvalidRailwayPhoneNumber");
    public static Error InvalidCityPhoneNumber => Error.Validation($"{errorPrefix}.InvalidCityPhoneNumber");

    public static Error AccountExists => Error.Conflict($"{errorPrefix}.AccountExists");

    public static Error InvalidCredentials => Error.Unauthorized($"{errorPrefix}.InvalidCredentials");
    public static Error InvalidPassword => Error.Validation($"{errorPrefix}.InvalidPassword");
    public static Error NumberAlreadyAssignedToUser => Error.Conflict($"{errorPrefix}.NumberAlreadyAssignedToUser");
    public static Error PhoneNumberNotAssignedToPosition => Error.Validation($"{errorPrefix}.PhoneNumberNotAssignedToPosition");
    public static Error InvalidUserExternalId => Error.Validation($"{errorPrefix}.InvalidUserExternalId");
}