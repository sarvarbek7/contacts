using Contacts.Domain.Users;
using Contacts.Infrastructure.Persistance;
using Infrastructure.Repositories;

namespace Contacts.Infrastructure.Repositories;

internal class UserRepository(AppDbContext context) : Repository<AppDbContext, 
    User, int>(context), IRepository< User, int>;