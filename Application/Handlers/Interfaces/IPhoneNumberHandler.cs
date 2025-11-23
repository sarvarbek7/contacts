using Application.Common;
using Contacts.Application.Handlers.Messages.PhoneNumbers;
using Contacts.Application.Handlers.Responses;
using Contacts.Domain.PhoneNumbers;
using ErrorOr;

namespace Contacts.Application.Handlers.Interfaces;

public interface IPhoneNumberHandler
{
    Task<ErrorOr<Success>> HandlePositionUserAssignPhoneNumber(AssignPositionUserPhoneNumberMessage message, CancellationToken cancellationToken = default);
    Task<ErrorOr<Success>> HandleUserAssignPhoneNumber(AssignUserPhoneNumberMessage message, CancellationToken cancellationToken = default);
    Task<ErrorOr<Success>> HandlePositionAssignPhoneNumber(AssignPositionPhoneNumberMessage message, CancellationToken cancellationToken = default);
    Task<ErrorOr<Success>> HandleRemoveUserPhoneNumber(RemoveUserPhoneNumberMessage message, CancellationToken cancellationToken = default);
    Task<ErrorOr<Success>> HandleRemovePositionUserPhoneNumber(RemovePositionUserPhoneNumberMessage message, CancellationToken cancellationToken = default);
    Task<ErrorOr<Success>> HandleRemovePhoneNumberFromPosition(RemovePositionPhoneNumberMessage message, CancellationToken cancellationToken = default);
    Task<ErrorOr<Created>> HandleCreate(CreatePhoneNumberMessage message, CancellationToken cancellationToken = default);
    Task<ErrorOr<Updated>> HandleUpdate(UpdatePhoneNumberMessage message, CancellationToken cancellationToken = default);
    Task<ErrorOr<Deleted>> HandleDelete(DeletePhoneNumberMessage message, CancellationToken cancellationToken = default);
    Task<ListResult<PhoneNumberListItemWithPosition>> HandleList(ListPhoneNumbersMessage message, CancellationToken cancellationToken = default);
    Task<List<WorkerWithPhoneNumber>> HandlePositionPhoneNumbers(ListPhoneNumbersForPositionMessage message, CancellationToken cancellationToken = default);
}