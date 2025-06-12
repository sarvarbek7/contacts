namespace Contacts.Contracts.Handbooks;

public record HandbookItemTranslationDto(string Alias,
                                         string Language,
                                         bool IsDefault);