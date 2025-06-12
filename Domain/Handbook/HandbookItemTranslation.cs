using Domain.Entities;
using Domain.Enums;

namespace Contacts.Domain.Handbook;

public class HandbookItemTranslation : ITranslation<HandbookItem>
{
    public required string Alias { get; set; }
    public Language Language { get; set; }
    public bool IsDefault { get; set; }
}
