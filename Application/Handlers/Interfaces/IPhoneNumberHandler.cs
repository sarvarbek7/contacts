using Contacts.Application.Handlers.Messages.PhoneNumbers;
using Contacts.Domain.PhoneNumbers;
using ErrorOr;

namespace Contacts.Application.Handlers.Interfaces;

public interface IPhoneNumberHandler
{
    Task<ErrorOr<Created>>  HandleCreate(CreatePhoneNumberMessage message, CancellationToken cancellationToken = default);
    Task<ErrorOr<Updated>> HandleUpdate(UpdatePhoneNumberMessage message, CancellationToken cancellationToken = default);
    Task<ErrorOr<Deleted>> HandleDelete(DeletePhoneNumberMessage message, CancellationToken cancellationToken = default);
    Task<ErrorOr<PhoneNumber>> HandleGetById(GetPhoneNumberByIdMessage message, CancellationToken cancellationToken = default);
}