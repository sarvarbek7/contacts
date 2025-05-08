using Application.Common;
using Contacts.Application.Handlers.Messages.PhoneNumbers;
using Contacts.Domain.PhoneNumbers;
using ErrorOr;

namespace Contacts.Application.Handlers.Interfaces;

public interface IPhoneNumberHandler
{
    Task<ErrorOr<Success>> HandleAssignPhoneNumber(AssignPhoneNumberMessage message, CancellationToken cancellationToken = default);
    Task<ErrorOr<Success>> HandleRemovePhoneNumber(RemovePhoneNumberMessage message, CancellationToken cancellationToken = default);
    Task<ErrorOr<Created>> HandleCreate(CreatePhoneNumberMessage message, CancellationToken cancellationToken = default);
    Task<ErrorOr<Updated>> HandleUpdate(UpdatePhoneNumberMessage message, CancellationToken cancellationToken = default);
    Task<ErrorOr<Deleted>> HandleDelete(DeletePhoneNumberMessage message, CancellationToken cancellationToken = default);
    Task<ErrorOr<PhoneNumber>> HandleGetById(GetPhoneNumberByIdMessage message, CancellationToken cancellationToken = default);
    Task<ListResult<PhoneNumber>> HandleList(ListPhoneNumbersMessage message, CancellationToken cancellationToken = default);
}