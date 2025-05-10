using Contacts.Domain.Accounts;
using Contacts.Infrastructure.Persistance;
using Infrastructure.Repositories;

namespace Contacts.Infrastructure.Repositories;

internal class AccountRepository(AppDbContext context) : Repository<AppDbContext, Account, int>(context), 
                                                         IRepository<Account, int>;