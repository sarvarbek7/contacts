using System.Linq.Expressions;
using Contacts.Application.Handlers.Messages.PhoneNumbers;
using Contacts.Contracts.PhoneNumbers;
using Contacts.Domain.PhoneNumbers;
using LinqKit;

namespace Contacts.Api.Mappings;

public static class PhoneNumberMapping
{
    public static CreatePhoneNumberMessage MapTo(this CreatePhoneNumberRequest request,
        int accountId)
    {
        return new CreatePhoneNumberMessage(request.Number, accountId);
    }

    public static UpdatePhoneNumberMessage MapTo(this UpdatePhoneNumberRequest request, Guid id, int accountId)
    {
        return new UpdatePhoneNumberMessage(id, request.Number, accountId);
    }

    public static ListPhoneNumbersMessage MapTo(this ListPhoneNumbersQuery query)
    {
        return new ListPhoneNumbersMessage(query.ToPagination(), query.Search);
    }

    public static Expression<Func<PhoneNumber, PhoneNumberListItem>> PhoneNumberToListItem =>
        x => new PhoneNumberListItem(x.Id,
                                     x.Number,
                                     x.ActiveAssignedUser == null ? null : UserMapping.DomainUserToContractUser.Invoke(x.ActiveAssignedUser));
}