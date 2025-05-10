using System.Security.Claims;

namespace Contacts.Api.Extensions;

public static class HttpContextExtensions
{
    public static int GetUserId(this HttpContext context)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("Can not find userId");

        return int.Parse(userId);
    }
}