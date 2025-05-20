using ErrorOr;

namespace Contacts.Application.Common.Errors;

public static class ApplicationErrors
{
    public static readonly string errorPrefix = "ApplicationErrors";

    public static Error ExternalServerError => Error.Failure($"{errorPrefix}.ExternalServerError");

    public static Error UserNotFoundInHRMSystem => Error.Validation($"{errorPrefix}.UserNotFoundInHRMSystem");
    public static Error PhoneNumberAlreadyExists => Error.Conflict($"{errorPrefix}.PhoneNumberAlreadyExists");
    public static Error InvalidPhoneNumber => Error.Validation($"{errorPrefix}.InvalidPhoneNumber");

    public static Error AccountExists => Error.Conflict($"{errorPrefix}.AccountExists");

    public static Error InvalidCredentials { get; set; }
    public static Error InvalidPassword { get; set; }
    public static Error NumberAlreadyAssignedToUser { get; set; }
    public static Error PhoneNumberNotAssignedToPosition { get; set; }
    public static Error InvalidUserExternalId { get; internal set; }
}