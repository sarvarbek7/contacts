using Application.Validators;
using Contacts.Application.Common.Errors;
using Contacts.Domain.PhoneNumbers;
using ErrorOr;

namespace Contacts.Application.Validators;

internal class PhoneNumberValidator : BaseValidator<PhoneNumber, Guid>, IValidator<PhoneNumber, Guid>
{
    public new ErrorOr<Created> ValidateOnAdd(PhoneNumber model)
    {
        var validateNumber = ValidateNumberField(model);

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
        var validateNumber = ValidateNumberField(model);

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

    private static ErrorOr<Success> ValidateNumberField(PhoneNumber phoneNumber)
    {
        if (phoneNumber.Type is PhoneNumberType.Railway)
        {
            if (phoneNumber.Number is { Length: 5 } validLengthRailwayNumber)
            {
                if (validLengthRailwayNumber.All(char.IsNumber))
                {
                    return new Success();
                }
            }

            return ApplicationErrors.InvalidRailwayPhoneNumber;
        }
        else if (phoneNumber.Type is PhoneNumberType.City)
        {
            if (phoneNumber.Number is { Length: 9 } validLengthRailwayNumber)
            {
                if (validLengthRailwayNumber.All(char.IsNumber))
                {
                    int code = int.Parse(validLengthRailwayNumber[..2]);

                    if (code is > 60 and < 80)
                    {
                        return new Success();
                    }
                }
            }

            return ApplicationErrors.InvalidCityPhoneNumber;
        }
        else
        {
            return ApplicationErrors.InvalidRailwayPhoneNumber;
        }
    }
}
