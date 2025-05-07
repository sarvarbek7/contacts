using Application.Services.Foundations;
using Application.Validators;
using Contacts.Domain.Users;
using Infrastructure.Repositories;
using Infrastructure.Services.Foundations;

namespace Contacts.Infrastructure.Services.Foundations;

internal class UserService(IRepository<User, int> repository,
                           IValidator<User, int> validator) : BaseService<User, int>(repository,
                                                                                     validator), IBaseService<User, int>;