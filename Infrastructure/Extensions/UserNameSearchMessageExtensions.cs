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
#pragma warning disable CS8604 // Possible null reference argument.
                    predicate.Or(x => EF.Functions.ILike(x.ActiveAssignedPositionUser.LastName, $"%{translationResult.Latin}%"));

                    predicate.Or(x => EF.Functions.ILike(x.ActiveAssignedPositionUser.FirstName, $"%{translationResult.Latin}%"));

                    predicate.Or(x => EF.Functions.ILike(x.ActiveAssignedPositionUser.MiddleName, $"%{translationResult.Latin}%"));
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }
            }
        }

        return predicate;
    }
}