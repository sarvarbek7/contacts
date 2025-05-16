using System.Text.Json.Serialization;

namespace Contacts.Application.ProcessingServices.Models.Responses.HrmPro;

public record HrmListResponse<T>
{
    [JsonPropertyName("current_page")]
    public int Page { get; init; }

    [JsonPropertyName("total")]
    public int Total { get; init; }

    public List<T> Data { get; init; } = [];
}