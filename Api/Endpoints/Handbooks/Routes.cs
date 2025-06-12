namespace Contacts.Api.Endpoints.Handbooks;

public static class Routes
{
    public const string BaseUrl = "handbooks";

    public const string WithId = "{id:int}";
    public const string Items = $"{WithId}/items";
    public const string ItemsWithItemId = $"{WithId}/items/{{itemId:int}}";
}