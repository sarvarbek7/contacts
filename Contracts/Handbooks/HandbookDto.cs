namespace Contacts.Contracts.Handbooks;

public record HandbookDto(int Id, List<HandbookTranslationDto> Translations, IEnumerable<HandbookItemDto> Items);