using Contacts.Domain.PhoneNumbers;
using Contacts.Infrastructure.Persistance;
using Infrastructure.Repositories;

namespace Contacts.Infrastructure.Repositories;

internal class PhoneNumberRepository(AppDbContext context) : Repository<AppDbContext, 
    PhoneNumber, Guid>(context), IRepository<PhoneNumber, Guid>;