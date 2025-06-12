using Contacts.Domain.Handbook;

namespace Contacts.Application.Handlers.Messages.Handbooks;

public record CreateHandlebookItemMessage(int HandbookId,
                                          Guid PhoneNumberId,
                                          List<HandbookItemTranslation> Translations);