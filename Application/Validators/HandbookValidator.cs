using Application.Validators;
using Contacts.Domain.Handbook;

namespace Contacts.Application.Validators;

internal class HandbookValidator : BaseValidator<Handbook, int>,
                                   IValidator<Handbook, int>;
