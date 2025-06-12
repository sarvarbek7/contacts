using Contacts.Domain.Handbook;

namespace Contacts.Application.Handlers.Messages.Handbooks;

public record UpdateHandbookItemMessage(int HandbookId,
                                        int ItemId,
                                        string Number,
                                        List<HandbookItemTranslation> Translations);