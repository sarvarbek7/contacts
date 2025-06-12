using Contacts.Domain.Handbook;

namespace Contacts.Application.Handlers.Messages.Handbooks;

public record CreateHandbookMessage(List<HandbookTranslation> Translations);