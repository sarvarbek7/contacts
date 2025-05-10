using Application.Validators;
using Contacts.Domain.Accounts;
using Domain.Entities;
using ErrorOr;

namespace Contacts.Application.Validators;

internal abstract class BaseValidator<T, TId> : IValidator<T, TId>
    where T : class, IEntity<TId>
    where TId : struct
{
    public ErrorOr<Created> ValidateOnAdd(T model)
    {
        if (model is ICreatedByAccountAudit auditable)
        {
            if (auditable.CreatedById is null or 0)
            {
                throw new MissingAuditException();
            }

            if (auditable.CreatedAt == default)
            {
                // TODO: throw MissingAuditException

                throw new NotImplementedException();
            }
        }

        return new Created();
    }

    public ErrorOr<Deleted> ValidateOnDelete(T model)
    {
        if (model is IDeletedByAccountAudit auditable)
        {
            if (auditable.DeletedById is null or 0)
            {
                throw new MissingAuditException();
            }

            if (auditable.DeletedAt == default)
            {
                // TODO: throw MissingAuditException

                throw new NotImplementedException();
            }
        }

        return new Deleted();
    }

    public ErrorOr<Updated> ValidateOnUpdate(T model)
    {
        if (model is IUpdatedByAccountAudit auditable)
        {
            if (auditable.UpdatedById is null or 0)
            {
                throw new MissingAuditException();
            }

            if (auditable.UpdatedAt == default)
            {
                // TODO: throw MissingAuditException

                throw new NotImplementedException();
            }
        }

        return new Updated();
    }
}
