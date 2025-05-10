using Application.Services.Foundations;
using Application.Validators;
using Contacts.Domain.Accounts;
using Infrastructure.Repositories;
using Infrastructure.Services.Foundations;

namespace Contacts.Infrastructure.Services.Foundations;

internal class AccountService(IRepository<Account, int> repository, 
                              IValidator<Account, int> validator) : BaseService<Account, int>(repository, 
                                                                                              validator), 
                                                                    IBaseService<Account, int>;