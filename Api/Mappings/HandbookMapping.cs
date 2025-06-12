using Contacts.Application.Handlers.Messages.Handbooks;
using Contacts.Contracts.Handbooks;
using Contacts.Domain.Handbook;
using Domain.Enums;
using Mappings;

namespace Contacts.Api.Mappings;

public static class HandbookMapping
{
    public static HandbookTranslation MapTo(this HandbookTranslationDto dto)
        => new()
        {
            Name = dto.Name,
            IsDefault = dto.IsDefault,
            Language = Enum.Parse<Language>(dto.Language, ignoreCase: true),
        };

    public static HandbookTranslationDto MapTo(this HandbookTranslation translation)
        => new(translation.Name, translation.Language.ToString(), translation.IsDefault);
    
    public static HandbookItemTranslation MapTo(this HandbookItemTranslationDto dto)
        => new()
        {
            Alias = dto.Alias,
            IsDefault = dto.IsDefault,
            Language = Enum.Parse<Language>(dto.Language, ignoreCase: true),
        };
    
    public static HandbookItemTranslationDto MapTo(this HandbookItemTranslation translation)
        => new(translation.Alias, translation.Language.ToString(), translation.IsDefault);

    public static CreateHandbookMessage MapTo(this CreateHandbookRequest request)
        => new([.. request.Translations.Select(x => x.MapTo())]);

    public static UpdateHandbookMessage MapTo(this UpdateHandbookRequest request, int id)
        => new(id, [.. request.Translations.Select(x=>x.MapTo())]);

    public static CreateHandlebookItemMessage MapTo(this CreateHandbookItemRequest request,
                                                    int id)
        => new(id, request.PhoneNumberId, [.. request.Translations.Select(x=>x.MapTo())]);

    public static UpdateHandbookItemMessage MapTo(this UpdateHandbookItemRequest request,
                                                  int handbookId,
                                                  int itemId)
        => new(handbookId, itemId, [.. request.Translations.Select(x=>x.MapTo())]);

    public static HandbookItemDto MapTo(this HandbookItem item)
        => new(item.Id, new(item.PhoneNumberId, item.PhoneNumber!.Number), [.. item.Translations.Select(x=>x.MapTo())]);

    public static HandbookDto MapTo(this Handbook handbook)
        => new(handbook.Id, [.. handbook.Translations.Select(x=>x.MapTo())], handbook.Items.Select(x => x.MapTo()));

    public static ListHandbookMessage MapTo(this ListHandbooksRequest request)
    {
        var pagination = request.ToPagination();

        return new ListHandbookMessage(pagination, request.Search);
    }
}