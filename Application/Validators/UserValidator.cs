using Application.Validators;
using Contacts.Application.Common.Errors;
using Contacts.Domain.Users;
using ErrorOr;

namespace Contacts.Application.Validators;

internal class UserValidator : IValidator<User, int>
{
    public ErrorOr<Created> ValidateOnAdd(User model)
    {
        if (model.ExternalId == default)
        {
            return ApplicationErrors.InvalidUserExternalId;
        }

        return new Created();
    }

    public ErrorOr<Deleted> ValidateOnDelete(User model)
    {
        return new Deleted();
    }

    public ErrorOr<Updated> ValidateOnUpdate(User model)
    {
        return new Updated();
    }
}
