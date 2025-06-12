using Contacts.Domain.Handbook;
using Contacts.Infrastructure.Persistance;
using Infrastructure.Repositories;

namespace Contacts.Infrastructure.Repositories;

internal class HandbookRepository(AppDbContext context) : Repository<AppDbContext, Handbook, int>(context),
                                                          IRepository<Handbook, int>;