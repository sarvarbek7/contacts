namespace Contacts.Contracts.Users;

public record UserListItem
{
    public int Id { get; init; }
    public int ExternalId { get; init; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string? Photo { get; set; }
}