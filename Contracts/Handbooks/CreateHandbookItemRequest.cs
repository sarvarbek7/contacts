namespace Contacts.Contracts.Handbooks;

public record CreateHandbookItemRequest(Guid PhoneNumberId, List<HandbookItemTranslationDto> Translations);