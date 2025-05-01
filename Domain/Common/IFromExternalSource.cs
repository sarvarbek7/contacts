namespace Contacts.Domain.Common;

public interface IFromExternalSource<TExternalId>
    where TExternalId : struct
{
    TExternalId ExternalId { get; init; }
}