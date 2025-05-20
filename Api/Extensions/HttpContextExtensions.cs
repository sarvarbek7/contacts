using System.Security.Claims;
using Domain.Enums;

namespace Contacts.Api.Extensions;

public static class HttpContextExtensions
{
    public static int GetUserId(this HttpContext context)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("Can not find userId");

        return int.Parse(userId);
    }

    public static Language GetLanguage(this HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("lang", out var languages))
        {
            var lang = languages.FirstOrDefault();

            if (Enum.TryParse<Language>(lang, true, out var language))
            {
                return language;
            }
        }

        return Language.Uz;
    }
}