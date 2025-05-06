using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.Handlers.Messages.PhoneNumbers;
using Contacts.Domain.PhoneNumbers;
using ErrorOr;
using Infrastructure.Services.Foundations;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Infrastructure.Handlers;

class PhoneNumberHandler(BaseService<PhoneNumber, Guid> phoneNumberService) : IPhoneNumberHandler
{
    public async Task<ErrorOr<Created>> HandleCreate(CreatePhoneNumberMessage message,
                             CancellationToken cancellationToken = default)
    {
        var phoneNumber = message.MapToPhoneNumber();

        phoneNumber.CreatedById = message.UserAccountIdWhoDoesAction;
        phoneNumber.CreatedAt = DateTime.UtcNow;

        bool phoneNumberExists = await phoneNumberService.GetAll(x => x.Number == phoneNumber.Number).AnyAsync(cancellationToken);

        if (phoneNumberExists)
        {
            // TODO: add error already exists
            throw new NotImplementedException();
        }

        return await phoneNumberService.Add(phoneNumber, true, cancellationToken);
    }

    public async Task<ErrorOr<Deleted>> HandleDelete(DeletePhoneNumberMessage message, CancellationToken cancellationToken = default)
    {
        var errorOrStoredPhoneNumber = await phoneNumberService.GetById(message.Id,
                                                                 tracked: true,
                                                                 cancellationToken: cancellationToken);

        return await errorOrStoredPhoneNumber.MatchAsync<ErrorOr<Deleted>>(async v => {
            return await phoneNumberService.Delete(v, true, cancellationToken);
        }, async e => {
            await Task.CompletedTask;
            
            return e;
        });
    }

    public Task HandleGetById(GetPhoneNumberByIdMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task HandleUpdate(UpdatePhoneNumberMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
