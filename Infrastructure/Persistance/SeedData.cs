using Contacts.Domain.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Contacts.Infrastructure.Persistance;

public static class Seeder
{
    private static readonly DateTime _createdAt = new(year: 2025,
                                                      month: 05,
                                                      day: 10,
                                                      hour: 0,
                                                      minute: 0,
                                                      second: 0,
                                                      kind: DateTimeKind.Utc);

    private static IEnumerable<Account> GetAccounts()
    {
        yield return new Account()
        {
            Id = -1,
            Login = "Admin",
            Role = Role.SuperAdmin,
            CreatedAt = _createdAt,
            Password = "$2y$10$nrQfT/6xJ4ELkg5NngMDBezvUjsOnP0xvsvQgH.ARv0yBUN23dwv6" //123Admin321
        };
    }

    public static async Task SeedData(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var accounts = GetAccounts().ToList();

        var storedAccounts = await dbContext.Set<Account>()
                                            .Where(x => accounts.Select(a => a.Id).Contains(x.Id))
                                            .ToListAsync();

        List<Account> notStoredAccounts = [];

        foreach (var account in accounts)
        {
            var storedAccount = storedAccounts.FirstOrDefault(x => x.Id == account.Id);

            if (storedAccount is null)
            {
                notStoredAccounts.Add(account);
            }
            else
            {
                storedAccount.Login = account.Login;
                storedAccount.Password = account.Password;
                storedAccount.Role = account.Role;
            }
        }

        if (notStoredAccounts.Count > 0)
        {
            await dbContext.Set<Account>()
                           .AddRangeAsync(notStoredAccounts);
        }

        await dbContext.SaveChangesAsync();
    }
}