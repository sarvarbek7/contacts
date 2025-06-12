using Domain.Entities;
using Domain.Enums;

namespace Contacts.Domain.Handbook;

public class HandbookTranslation : ITranslation<Handbook>
{
    public required string Name { get; set; }
    public Language Language { get; set; }
    public bool IsDefault { get; set; }
}
