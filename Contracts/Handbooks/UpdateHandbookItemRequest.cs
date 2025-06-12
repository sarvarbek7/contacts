namespace Contacts.Contracts.Handbooks;

public record UpdateHandbookItemRequest(string Number, List<HandbookItemTranslationDto> Translations);