using Contacts.Domain.PhoneNumbers;
using Domain.Entities;

namespace Contacts.Domain.Handbook;

public class HandbookItem : IEntity<int>, ITranslatable<HandbookItem, HandbookItemTranslation>
{
    public int Id { get; init; }
    public int HandbookId { get; set; }
    public required string Number { get; set; }
    public List<HandbookItemTranslation> Translations { get; set; } = [];
}