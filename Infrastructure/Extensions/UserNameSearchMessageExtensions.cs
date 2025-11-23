using Application.ProcessingServices;
using Contacts.Application.Handlers.Messages.PhoneNumbers;
using Contacts.Domain.PhoneNumbers;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Infrastructure.Extensions;

public static class UserNameSearchMessageExtensions
{
    public static ExpressionStarter<PhoneNumber> BuildPredicate(this IUserNameSearchMessage message, ITranslationService translationService)
    {
        ExpressionStarter<PhoneNumber> predicate = PredicateBuilder.New<PhoneNumber>();

        if (message.User?.Trim().ToLower() is { } user)
        {
            var names = user.Split(' ', StringSplitOptions.RemoveEmptyEntries |
                                        StringSplitOptions.TrimEntries);

            foreach (var name in names)
            {
                var temp = name;

                if (temp.Length >= 3)
                {
                    var translationResult = translationService.Translate(temp);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    predicate.Or(x => x.AssignedPositions.Any(x => x.Users.Any(u =>
                        EF.Functions.ILike(u.FirstName, $"%{translationResult.Latin}%") ||
                        EF.Functions.ILike(u.LastName, $"%{translationResult.Latin}%") ||
                        EF.Functions.ILike(u.MiddleName, $"%{translationResult.Latin}%"))));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }
            }
        }

        return predicate;
    }
}