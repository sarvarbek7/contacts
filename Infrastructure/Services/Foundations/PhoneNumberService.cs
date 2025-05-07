using Application.Services.Foundations;
using Application.Validators;
using Contacts.Domain.PhoneNumbers;
using Infrastructure.Repositories;
using Infrastructure.Services.Foundations;

namespace Contacts.Infrastructure.Services.Foundations;

internal class PhoneNumberService(IRepository<PhoneNumber, Guid> repository,
                                  IValidator<PhoneNumber, Guid> validator) : BaseService<PhoneNumber, Guid>(repository, 
                                                                                                            validator), 
                                                                             IBaseService<PhoneNumber, Guid>;