namespace Contacts.Contracts.Handbooks;

public record HandbookItemDto(int Id, string Number, List<HandbookItemTranslationDto> Translations);