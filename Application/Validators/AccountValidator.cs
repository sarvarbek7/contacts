using Application.Validators;
using Contacts.Domain.Accounts;

namespace Contacts.Application.Validators;

internal class AccountValidator :  BaseValidator<Account, int>, IValidator<Account, int>
{
}
