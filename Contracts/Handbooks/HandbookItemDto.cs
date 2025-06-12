namespace Contacts.Contracts.Handbooks;

public record HandbookItemDto(int Id, PhoneNumber PhoneNumber, List<HandbookItemTranslationDto> Translations);