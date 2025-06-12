namespace Contacts.Contracts.Handbooks;

public record HandbookTranslationDto(string Name,
                                     string Language,
                                     bool IsDefault);