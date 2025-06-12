using Contacts.Domain.Handbook;

namespace Contacts.Application.Handlers.Messages.Handbooks;

public record UpdateHandbookMessage(int Id, List<HandbookTranslation> Translations);