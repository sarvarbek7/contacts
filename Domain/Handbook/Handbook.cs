using Domain.Entities;

namespace Contacts.Domain.Handbook;

public class Handbook : IEntity<int>, ITranslatable<Handbook, HandbookTranslation>
{
    public int Id { get; init; }
    public List<HandbookItem> Items { get; set; } = [];
    public List<HandbookTranslation> Translations { get; set; } = [];
}
