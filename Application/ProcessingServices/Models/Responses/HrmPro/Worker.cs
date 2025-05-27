using System.Text.Json.Serialization;

namespace Contacts.Application.ProcessingServices.Models.Responses.HrmPro;

public record Worker
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("last_name")]
    public required string LastName { get; init; }

    [JsonPropertyName("first_name")]
    public required string FirstName { get; init; }


    [JsonPropertyName("middle_name")]
    public required string MiddleName { get; init; }

    [JsonPropertyName("photo")]
    public required string Photo { get; init; }

    [JsonPropertyName("phones")]
    public List<object> Phones { get; init; } = [];

    public void HideNumber()
    {
        for (int i = 0; i < Phones.Count; i++)
        {
            string phoneNumber = (Phones[i] ?? "").ToString()!;

            int length = phoneNumber.Length;

            if (length > 2)
            {
                phoneNumber = phoneNumber[..2] + new string('*', length - 2);
            }

            Phones[i] = phoneNumber;
        }
    }
}