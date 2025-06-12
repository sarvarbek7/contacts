using Application.Common;
using Application.Common.Extensions;
using Application.Services.Foundations;
using Contacts.Application.Common.Errors;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.Handlers.Messages.Handbooks;
using Contacts.Domain.Handbook;
using Contacts.Domain.PhoneNumbers;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Infrastructure.Handlers;

internal class HandbookHandler(IBaseService<Handbook, int> service,
                               IBaseService<PhoneNumber, Guid> phoneNumberService) : IHandbookHandler
{
    public async Task<ErrorOr<Handbook>> HandleCreateHandbook(CreateHandbookMessage message, CancellationToken cancellationToken = default)
    {
        var handbook = new Handbook()
        {
            Translations = message.Translations,
        };

        return (await service.Add(handbook, saveChanges: true, cancellationToken))
                    .Match<ErrorOr<Handbook>>(x => handbook, e => e);
    }

    public async Task<ErrorOr<HandbookItem>> HandleCreateHandbookItem(CreateHandlebookItemMessage message, CancellationToken cancellationToken = default)
    {
        var errorOrHandleBook = await service.GetById(message.HandbookId,
                                                      tracked: true,
                                                      includeStrings: [nameof(Handbook.Items)],
                                                      cancellationToken: cancellationToken);

        if (errorOrHandleBook.IsError)
        {
            return errorOrHandleBook.FirstError;
        }

        var handleBook = errorOrHandleBook.Value;

        var errorOrPhoneNumber = await phoneNumberService.GetById(message.PhoneNumberId, tracked: true, cancellationToken: cancellationToken);

        if (errorOrPhoneNumber.IsError)
        {
            return errorOrPhoneNumber.FirstError;
        }

        var item = new HandbookItem()
        {
            PhoneNumber = errorOrPhoneNumber.Value,
            Translations = message.Translations,
        };

        handleBook.Items.Add(item);

        await service.SaveChanges(cancellationToken);

        return item;
    }

    public async Task<ErrorOr<Success>> HandleDeleteHandbook(DeleteHandbookMessage message, CancellationToken cancellationToken = default)
    {
        var errorOrHandleBook = await service.GetById(message.Id,
                                                      tracked: true,
                                                      cancellationToken: cancellationToken);

        if (errorOrHandleBook.IsError)
        {
            return errorOrHandleBook.FirstError;
        }

        var handleBook = errorOrHandleBook.Value;

        return (await service.Delete(handleBook, true, cancellationToken))
            .Match<ErrorOr<Success>>(v => new Success(), e => e);
    }

    public async Task<ErrorOr<Success>> HandleDeleteHandbookItem(DeleteHandbookItemMessage message, CancellationToken cancellationToken = default)
    {
        var errorOrHandleBook = await service.GetById(message.Id,
                                                      tracked: true,
                                                      cancellationToken: cancellationToken);

        if (errorOrHandleBook.IsError)
        {
            return errorOrHandleBook.FirstError;
        }

        var handbook = errorOrHandleBook.Value;

        var item = handbook.Items.FirstOrDefault(x => x.PhoneNumberId == message.PhoneNumberId);

        if (item is null)
        {
            return ApplicationErrors.HandbookItemNotFound;
        }

        handbook.Items.Remove(item);

        await service.SaveChanges(cancellationToken);

        return new Success();
    }

    public async Task<ErrorOr<Handbook>> HandleGetHandleById(int Id, CancellationToken cancellationToken = default)
    {
        var errorOrHandleBook = await service.GetById(Id,
                                                      tracked: true,
                                                      includeStrings: [nameof(Handbook.Items), $"{nameof(Handbook.Items)}.{nameof(HandbookItem.PhoneNumber)}"],
                                                      cancellationToken: cancellationToken);

        if (errorOrHandleBook.IsError)
        {
            return errorOrHandleBook.FirstError;
        }

        var handleBook = errorOrHandleBook.Value;

        return handleBook;
    }

    public async Task<ListResult<Handbook>> HandleListHandbooks(ListHandbookMessage message, CancellationToken cancellationToken = default)
    {
        var query = service.GetAll(tracked: false);

        if (message.Search is { } search)
        {
            query = query.Where(x => x.Translations.Any(t => EF.Functions.ILike(t.Name, $"%{search}%")));
        }

        var total = await query.CountAsync(cancellationToken);

        query = query.Include(x => x.Items)
                     .ThenInclude(i => i.PhoneNumber);

        query = query.Paged(message.Pagination);

        var data = await query.ToListAsync(cancellationToken);

        return ListResult<Handbook>.FromCollection(data, message.Pagination, total);
    }

    public async Task<ErrorOr<Handbook>> HandleUpdateHandbook(UpdateHandbookMessage message, CancellationToken cancellationToken = default)
    {
        var errorOrHandleBook = await service.GetById(message.Id,
                                                      tracked: true,
                                                      cancellationToken: cancellationToken);

        if (errorOrHandleBook.IsError)
        {
            return errorOrHandleBook.FirstError;
        }

        var handbook = errorOrHandleBook.Value;

        handbook.Translations = message.Translations;

        await service.SaveChanges(cancellationToken);

        return handbook;
    }

    public async Task<ErrorOr<HandbookItem>> HandleUpdateHandbookItem(UpdateHandbookItemMessage message, CancellationToken cancellationToken = default)
    {
        var errorOrHandleBook = await service.GetById(message.HandbookId,
                                                      tracked: true,
                                                      includeStrings: [nameof(Handbook.Items), $"{nameof(Handbook.Items)}.{nameof(HandbookItem.PhoneNumber)}"],
                                                      cancellationToken: cancellationToken);

        if (errorOrHandleBook.IsError)
        {
            return errorOrHandleBook.FirstError;
        }

        var handleBook = errorOrHandleBook.Value;

        var item = handleBook.Items.FirstOrDefault(x => x.Id == message.ItemId);

        if (item is null)
        {
            return ApplicationErrors.HandbookItemNotFound;
        }

        item.Translations = message.Translations;

        await service.SaveChanges(cancellationToken);

        return item;
    }
}
