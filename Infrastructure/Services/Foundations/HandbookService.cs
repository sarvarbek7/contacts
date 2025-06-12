using Application.Services.Foundations;
using Application.Validators;
using Contacts.Domain.Handbook;
using Infrastructure.Repositories;
using Infrastructure.Services.Foundations;

namespace Contacts.Infrastructure.Services.Foundations;

internal class HandbookService(IRepository<Handbook, int> repository,
                               IValidator<Handbook, int> validator) : BaseService<Handbook, int>(repository, validator),
                                                                      IBaseService<Handbook, int>;