using Application.Validators;
using Contacts.Domain.PhoneNumbers;
using ErrorOr;

namespace Contacts.Application.Validators;

internal class PhoneNumberValidator : BaseValidator<PhoneNumber, Guid>, IValidator<PhoneNumber, Guid>
{
    public new ErrorOr<Created> ValidateOnAdd(PhoneNumber model)
    {
        var validateNumber = ValidateNumberField(model.Number);

        List<Error> errors = [];

        if (validateNumber.IsError)
        {
            errors.Add(validateNumber.FirstError);
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return base.ValidateOnAdd(model);
    }

    public new ErrorOr<Deleted> ValidateOnDelete(PhoneNumber model)
    {
        return base.ValidateOnDelete(model);
    }

    public new ErrorOr<Updated> ValidateOnUpdate(PhoneNumber model)
    {
        var validateNumber = ValidateNumberField(model.Number);

        List<Error> errors = [];

        if (validateNumber.IsError)
        {
            errors.Add(validateNumber.FirstError);
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return base.ValidateOnUpdate(model);
    }

    private static ErrorOr<Success> ValidateNumberField(string number)
    {
        if (number is { Length: 5 } validLengthNumber)
        {
            if (!validLengthNumber.All(char.IsNumber))
            {
                return new Success();
            }
        }

        // TODO: return InvalidPhoneNumber error
        throw new NotImplementedException();
    }
}
