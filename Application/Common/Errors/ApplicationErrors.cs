using ErrorOr;

namespace Contacts.Application.Common.Errors;

public static class ApplicationErrors
{
    public static readonly string errorPrefix = nameof(ApplicationErrors);

    public static Error ExternalServerError => Error.Failure($"{errorPrefix}.ExternalServerError");

    public static Error UserNotFoundInHRMSystem => Error.Validation($"{errorPrefix}.UserNotFoundInHRMSystem");
}