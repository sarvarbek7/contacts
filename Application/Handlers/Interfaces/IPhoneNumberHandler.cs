using Contacts.Application.Handlers.Messages.PhoneNumbers;
using ErrorOr;

namespace Contacts.Application.Handlers.Interfaces;

public interface IPhoneNumberHandler
{
    Task<ErrorOr<Created>>  HandleCreate(CreatePhoneNumberMessage message, CancellationToken cancellationToken = default);
    Task HandleUpdate(UpdatePhoneNumberMessage message, CancellationToken cancellationToken = default);
    Task<ErrorOr<Deleted>> HandleDelete(DeletePhoneNumberMessage message, CancellationToken cancellationToken = default);
    Task HandleGetById(GetPhoneNumberByIdMessage message, CancellationToken cancellationToken = default);
}