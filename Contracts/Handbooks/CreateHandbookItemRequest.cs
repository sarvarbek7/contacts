namespace Contacts.Contracts.Handbooks;

public record CreateHandbookItemRequest(string Number, List<HandbookItemTranslationDto> Translations);