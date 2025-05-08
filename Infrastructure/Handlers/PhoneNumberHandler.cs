using System.Linq.Expressions;
using System.Net.Cache;
using Application.Common;
using Application.Errors;
using Application.Services.Foundations;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.Handlers.Messages.PhoneNumbers;
using Contacts.Domain.PhoneNumbers;
using ErrorOr;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Infrastructure.Handlers;

class PhoneNumberHandler(IBaseService<PhoneNumber, Guid> phoneNumberService,
                         IUserHandler userHandler) : IPhoneNumberHandler
{
    public async Task<ErrorOr<Success>> HandleAssignPhoneNumber(AssignPhoneNumberMessage message, CancellationToken cancellationToken = default)
    {
        var errorOrUser = await userHandler.HandleAddOrGetUser(message.User, cancellationToken);

        if (errorOrUser.IsError)
        {
            return errorOrUser.FirstError;
        }

        var user = errorOrUser.Value;

        var phoneNumber = await phoneNumberService.GetAll(x => x.Id == message.PhoneNumberId, tracked: true)
            .Include(x => x.ActiveAssignedUser)
            .Include(x => x.UsersHistory.Where(x => x.IsActive == true))
            .FirstOrDefaultAsync(cancellationToken);

        if (phoneNumber is null)
        {
            return ApplicationErrors.EntityNotFound<PhoneNumber, Guid>(message.PhoneNumberId);
        }

        if (phoneNumber.ActiveAssignedUser is not null)
        {
            // TODO: already assigned first retain
            throw new NotImplementedException();
        }

        phoneNumber.AssignUser(user, message.UserAccountIdWhoDoesAction);

        await phoneNumberService.SaveChanges(cancellationToken);

        return new Success();
    }

    public async Task<ErrorOr<Created>> HandleCreate(CreatePhoneNumberMessage message,
                             CancellationToken cancellationToken = default)
    {
        var phoneNumber = message.MapToPhoneNumber();

        // phoneNumber.CreatedById = message.UserAccountIdWhoDoesAction;
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

        return await errorOrStoredPhoneNumber.MatchAsync<ErrorOr<Deleted>>(async v =>
            {
                // v.DeletedById = message.UserAccountIdWhoDoesAction;
                v.DeletedAt = DateTime.UtcNow;

                return await phoneNumberService.Delete(v, true, cancellationToken);
            }, async e =>
            {
                await Task.CompletedTask;

                return e;
            });
    }

    public async Task<ErrorOr<PhoneNumber>> HandleGetById(GetPhoneNumberByIdMessage message, CancellationToken cancellationToken = default)
    {
        return await phoneNumberService.GetById(message.Id,
                                                tracked: true,
                                                includeStrings: [nameof(PhoneNumber.ActiveAssignedUser), 
                                                $"{nameof(PhoneNumber.UsersHistory)}.{nameof(UserPhoneNumber.User)}"],
                                                cancellationToken: cancellationToken);

    }

    public async Task<ListResult<PhoneNumber>> HandleList(ListPhoneNumbersMessage message, CancellationToken cancellationToken = default)
    {
        var query = phoneNumberService.GetAll(tracked: false).AsExpandable();

        if (message.Number?.Trim().ToLower() is { } number)
        {
            query = query.Where(x => EF.Functions.ILike(x.Number, $"%number%"));
        }

        if (message.User?.Trim().ToLower() is { } user)
        {
            var names = user.Split(' ', StringSplitOptions.RemoveEmptyEntries |
                                        StringSplitOptions.TrimEntries);

            var predicate = PredicateBuilder.New<PhoneNumber>();

            foreach (var name in names)
            {
                var temp = name;

                if (temp.Length >= 3)
                {
                    predicate.Or(x => EF.Functions.ILike(x.ActiveAssignedUser.LastName, $"%{temp}%"));
                }

            }

            foreach (var name in names)
            {
                var temp = name;

                if (temp.Length >= 3)
                {
                    predicate.Or(x => EF.Functions.ILike(x.ActiveAssignedUser.FirstName, $"%{temp}%"));
                }
            }

            foreach (var name in names)
            {
                var temp = name;

                if (temp.Length >= 3)
                {
                    predicate.Or(x => EF.Functions.ILike(x.ActiveAssignedUser.MiddleName, $"%{temp}%"));
                }
            }

            if (predicate.IsStarted)
            {
                query = query.Where(predicate);

            }

            //             var searchQuery = string.Join(" & ", names);

            // #pragma warning disable CS8602 // Dereference of a possibly null reference.
            //             query = query.Where(x => EF.Functions.ToTsVector(x.ActiveAssignedUser!.LastName + " " + x.ActiveAssignedUser!.FirstName + " " + x.ActiveAssignedUser!.MiddleName)
            //             .Matches(EF.Functions.ToTsQuery("simple", searchQuery)));
            // #pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        var total = await query.CountAsync(cancellationToken);

        query = query.Paged(message.Pagination);

        return new ListResult<PhoneNumber>(query, message.Pagination, total);
    }

    public async Task<ErrorOr<Success>> HandleRemovePhoneNumber(RemovePhoneNumberMessage message, CancellationToken cancellationToken = default)
    {
        var phoneNumber = await phoneNumberService.GetAll(x => x.Id == message.Id, tracked: true)
            .Include(x => x.UsersHistory.Where(x => x.IsActive == true))
            .FirstOrDefaultAsync(cancellationToken);

        if (phoneNumber is null)
        {
            return ApplicationErrors.EntityNotFound<PhoneNumber, Guid>(message.Id);
        }

        phoneNumber.UnAssign(message.UserAccountIdWhoDoesAction);

        await phoneNumberService.SaveChanges(cancellationToken);

        return new Success();
    }

    public async Task<ErrorOr<Updated>> HandleUpdate(UpdatePhoneNumberMessage message, CancellationToken cancellationToken = default)
    {
        var errorOrStoredPhoneNumber = await phoneNumberService.GetById(message.Id,
                                                                 tracked: true,
                                                                 cancellationToken: cancellationToken);

        return await errorOrStoredPhoneNumber.MatchAsync<ErrorOr<Updated>>(async v =>
            {
                v.Number = message.Number;
                // v.UpdatedById = message.UserAccountIdWhoDoesAction;
                v.UpdatedAt = DateTime.UtcNow;

                return await phoneNumberService.Update(v, true, cancellationToken);
            }, async e =>
            {
                await Task.CompletedTask;

                return e;
            });
    }
}