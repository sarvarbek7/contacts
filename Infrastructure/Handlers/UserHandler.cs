using Application.Common;
using Application.Common.Extensions;
using Application.ProcessingServices;
using Application.Services.Foundations;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.Handlers.Messages.Users;
using Contacts.Domain.Users;
using ErrorOr;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Infrastructure.Handlers;

class UserHandler(IBaseService<User, int> userService,
                  ITranslationService translationService) : IUserHandler
{
    public async Task<ErrorOr<User>> HandleAddOrGetUser(User user, CancellationToken cancellationToken = default)
    {
        var storedUser = await userService.GetAll(x => x.ExternalId == user.ExternalId)
            .FirstOrDefaultAsync(cancellationToken);

        if (storedUser is not null)
        {
            return storedUser;
        }

        return (await userService.Add(user,
                                      true,
                                      cancellationToken)).Match<ErrorOr<User>>(v => user, e => e);
    }

    public async Task<User?> HandleGetUserByExternalId(GetUserByExternalIdMessage message, CancellationToken cancellationToken = default)
    {
        var query = userService.GetAll(x => x.ExternalId == message.ExternalId, tracked: false);

        query = query.Include(x => x.ActivePhoneNumbers)
                     .Include(x => x.PhoneNumberHistory)
                     .ThenInclude(h => h.PhoneNumber);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ListResult<User>> HandleSelectUsers(SelectUsersMessage message, CancellationToken cancellationToken = default)
    {
        var query = userService.GetAll(tracked: false);
        ExpressionStarter<User> predicate = PredicateBuilder.New<User>();

        if (message.Search?.Trim().ToLower() is { } search)
        {
            var translation = translationService.Translate(search);


            var names = search.Split(' ', StringSplitOptions.RemoveEmptyEntries |
                                        StringSplitOptions.TrimEntries);

            foreach (var name in names)
            {
                var temp = name;

                if (temp.Length >= 3)
                {
                    var translationResult = translationService.Translate(temp);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
                    predicate.Or(x => EF.Functions.ILike(x.LastName, $"%{translationResult.Latin}%"));

                    predicate.Or(x => EF.Functions.ILike(x.FirstName, $"%{translationResult.Latin}%"));

                    predicate.Or(x => EF.Functions.ILike(x.MiddleName, $"%{translationResult.Latin}%"));
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }
            }
        }

        if (predicate.IsStarted)
        {
            query = query.Where(predicate);
        }

        if (message.HaveNumber.HasValue)
        {
            if (message.HaveNumber.Value)
            {
                query = query.Where(x => x.ActivePhonePositionNumbers.Any());
            }
        }

        var count = await query.CountAsync(cancellationToken);

        query = query.OrderBy(x => x.LastName);

        query = query.Paged(message.Pagination);

        return message.ToListResultWithQuery(query, count);
    }
}
